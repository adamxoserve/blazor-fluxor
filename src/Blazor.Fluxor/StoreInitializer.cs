using Blazor.Fluxor.Exceptions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace Blazor.Fluxor
{
	/// <summary>
	/// Initializes the stoe for the current user. This should be placed in the App.razor component.
	/// </summary>
	public class StoreInitializer : ComponentBase
	{
		[Inject]
		private IStore Store { get; set; }

		[Inject]
		private IJSRuntime JSRuntime { get; set; }

		private string Scripts;

		/// <summary>
		/// Retrieves supporting JavaScript for any Middleware
		/// </summary>
		protected override void OnInitialized()
		{
			Scripts = Store.GetScripts();
			base.OnInitialized();
		}

		/// <summary>
		/// Renders the supporting JavaScript for any Middleware
		/// </summary>
		/// <param name="builder">The builder</param>
		protected override void BuildRenderTree(RenderTreeBuilder builder)
		{
			base.BuildRenderTree(builder);
			builder.AddMarkupContent(0, Scripts);
		}

		/// <summary>
		/// Executes any supporting JavaScript required for Middleware
		/// </summary>
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
