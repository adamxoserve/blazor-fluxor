using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Blazor.Fluxor;

namespace AzureADB2C.Client
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			var builder = WebAssemblyHostBuilder.CreateDefault(args);
			builder.RootComponents.Add<App>("app");

			builder.Services.AddBaseAddressHttpClient();
			builder.Services.AddMsalAuthentication(options =>
			{
				var instance = "";
				var clientApplicationId = "";
				var serverApplicationId = "";
				var domain = "";
				var signUpOrSignInPolicy = "";
				var serverAppId = $"{serverApplicationId}";
				var defaultScope = "";

				var authentication = options.ProviderOptions.Authentication;
				authentication.Authority = $"{instance}{domain}/{signUpOrSignInPolicy}";

				authentication.ClientId = clientApplicationId;
				authentication.ValidateAuthority = false;
				options.ProviderOptions.DefaultAccessTokenScopes.Add($"{serverAppId}/{defaultScope}");
			});

			builder.Services.AddFluxor(options => options
				.UseDependencyInjection(typeof(Program).Assembly)
				.AddMiddleware<Blazor.Fluxor.Routing.RoutingMiddleware>()
			);

			await builder.Build().RunAsync();
		}
	}
}
