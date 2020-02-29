using Blazor.Fluxor.UnitTests.SupportFiles;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace Blazor.Fluxor.UnitTests.StoreTests
{
	public partial class StoreTests
	{
		public class AddMiddleware
		{
			[Fact]
			public async Task ActivatesMiddleware_WhenPageHasAlreadyLoaded()
			{
				var subject = new Store();
				await subject.InitializeAsync();

				var mockMiddleware = new Mock<IMiddleware>();
				subject.AddMiddleware(mockMiddleware.Object);

				mockMiddleware
					.Verify(x => x.InitializeAsync(subject));
			}

			[Fact]
			public async Task CallsAfterInitializeAllMiddlewares_WhenPageHasAlreadyLoaded()
			{
				var subject = new Store();
				await subject.InitializeAsync();

				var mockMiddleware = new Mock<IMiddleware>();
				subject.AddMiddleware(mockMiddleware.Object);

				mockMiddleware
					.Verify(x => x.AfterInitializeAllMiddlewares());
			}
		}
	}
}
