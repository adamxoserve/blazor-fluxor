using Blazor.Fluxor.Exceptions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Blazor.Fluxor
{
	public class StoreInitializer : ComponentBase
	{
		[Inject]
		private IStore Store { get; set; }

		[Inject]
		private IJSRuntime JSRuntime { get; set; }

		private string Scripts;

		protected override void OnInitialized()
		{
			Scripts = Store.GetScripts();
			base.OnInitialized();
		}

		protected override void BuildRenderTree(RenderTreeBuilder builder)
		{
			base.BuildRenderTree(builder);
			builder.AddMarkupContent(0, Scripts);
		}

		protected override async Task OnAfterRenderAsync(bool firstRender)
		{
			await base.OnAfterRenderAsync(firstRender);
			if (firstRender)
			{
				try
				{
					bool success = await JSRuntime.InvokeAsync<bool>("tryInitializeFluxor");
					if (!success)
						throw new StoreInitializationException("Failed to initialize store");

					await Store.InitializeAsync();
				}
				catch (JSException err)
				{
					// An error in some JavaScript, cannot recover from this
					throw new StoreInitializationException("JavaScript error", err);
				}
				catch(TaskCanceledException)
				{
					// The browser has disconnected from a server-side-blazor app and can no longer be reached.
					// Swallow this exception as the store will be abandoned and garbage collected.
					return;
				}
				catch (Exception err)
				{
					throw new StoreInitializationException("Store initialization error", err);
				}
			}
		}
	}
}
