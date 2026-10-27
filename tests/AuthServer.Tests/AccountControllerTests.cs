using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace AuthServer.Tests;

public class AccountControllerTests
{
    [Fact]
    public void Login_ReturnsTheLoginView()
    {
        var store = new Mock<IUserStore<IdentityUser>>().Object;
        var userManager = new Mock<UserManager<IdentityUser>>(
            store, Mock.Of<IOptions<IdentityOptions>>(), Mock.Of<IPasswordHasher<IdentityUser>>(),
            Array.Empty<IUserValidator<IdentityUser>>(), Array.Empty<IPasswordValidator<IdentityUser>>(),
            Mock.Of<ILookupNormalizer>(), new IdentityErrorDescriber(), Mock.Of<IServiceProvider>(), Mock.Of<ILogger<UserManager<IdentityUser>>>()).Object;
        var signInManager = new Mock<SignInManager<IdentityUser>>(
            userManager, Mock.Of<IHttpContextAccessor>(), Mock.Of<IUserClaimsPrincipalFactory<IdentityUser>>(),
            Mock.Of<IOptions<IdentityOptions>>(), Mock.Of<ILogger<SignInManager<IdentityUser>>>(),
            Mock.Of<IAuthenticationSchemeProvider>(), Mock.Of<IUserConfirmation<IdentityUser>>()).Object;
        var controller = new AccountController(userManager, signInManager);

        var result = controller.Login("/connect/authorize");

        Assert.IsType<ViewResult>(result);
    }
}
