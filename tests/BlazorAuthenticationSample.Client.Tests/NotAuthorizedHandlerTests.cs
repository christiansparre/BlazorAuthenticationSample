using Microsoft.AspNetCore.Components.Testing;
using Moq;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Components;
using BlazorAuthenticationSample.Client.Features;
using BlazorAuthenticationSample.Client.Features.Security.Components;
using Shouldly;

namespace BlazorAuthenticationSample.Client.Tests
{
    public class NotAuthorizedHandlerTests
    {
        private TestHost testHost = new TestHost();
        private Mock<AuthenticationStateProvider> _authenticationStateProvider;
        private TestNavigationManager _testNavigationManager;
        private SignInRedirectContext _signInRedirectContext;

        public NotAuthorizedHandlerTests()
        {
            _authenticationStateProvider = new Mock<AuthenticationStateProvider>();
            _testNavigationManager = new TestNavigationManager();
            _testNavigationManager.SetInitialized();
            _signInRedirectContext = new SignInRedirectContext();

            testHost.ConfigureServices(services =>
            {
                services.AddLogging();
                services.AddAuthorization(options =>
                {
                    options.AddPolicy("RequireAdmin", c => c.RequireRole("Admin"));
                });
                services.AddSingleton<SignInRedirectContext>(_signInRedirectContext);
                services.AddSingleton<NavigationManager>(_testNavigationManager);
                services.AddSingleton<AuthenticationStateProvider>(_authenticationStateProvider.Object);
            });
        }

        [Fact]
        public void ShouldRedirectToSignInPageIfNotAuthenticated()
        {
            _authenticationStateProvider.Setup(s => s.GetAuthenticationStateAsync()).ReturnsAsync(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity())));

            var component = testHost.AddComponent<NotAuthorizedHandler>();

            var navigation = _testNavigationManager.Navigations.Pop();
            navigation.uri.ShouldBe("/account/signin");
            navigation.forceLoad.ShouldBeFalse();
        }

        [Fact]
        public void ShouldSetReturnUrlOnSignInRedirectContextIfNotAuthenticated()
        {
            _authenticationStateProvider.Setup(s => s.GetAuthenticationStateAsync()).ReturnsAsync(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity())));

            var component = testHost.AddComponent<NotAuthorizedHandler>();

            _signInRedirectContext.ReturnUrl.ShouldBe("/test/test");
        }

        [Fact]
        public void ShouldShowNotAuthorizedMessageIfAuthenticatedButNotAuthorized()
        {
            _authenticationStateProvider.Setup(s => s.GetAuthenticationStateAsync()).ReturnsAsync(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim("test", "test") }, "Test"))));

            var component = testHost.AddComponent<NotAuthorizedHandler>();

            var element = component.Find("#not-authorized-message");

            element.ShouldNotBeNull();
        }
    }
}
