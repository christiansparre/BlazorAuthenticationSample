using System.Net;
using System.Security.Claims;
using BlazorAuthenticationSample.Client.Features.Security.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Shouldly;
using Xunit;

namespace BlazorAuthenticationSample.Client.Tests
{
    public class NotAuthorizedHandlerTests
    {
        private TestHost testHost = new TestHost();
        private Mock<AuthenticationStateProvider> _authenticationStateProvider;
        private TestNavigationManager _testNavigationManager;

        public NotAuthorizedHandlerTests()
        {
            _authenticationStateProvider = new Mock<AuthenticationStateProvider>();
            _testNavigationManager = new TestNavigationManager();
            _testNavigationManager.SetInitialized();

            testHost.ConfigureServices(services =>
            {
                services.AddLogging();
                services.AddAuthorization(options =>
                {
                    options.AddPolicy("RequireAdmin", c => c.RequireRole("Admin"));
                });
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
            navigation.uri.ShouldBe($"/account/signin?returnUrl={WebUtility.UrlEncode("/test/test")}");
            navigation.forceLoad.ShouldBeFalse();
        }

        [Fact]
        public void ShouldIncludeReturnUrlInRedirectToSignInPageIfNotAuthenticated()
        {
            _authenticationStateProvider.Setup(s => s.GetAuthenticationStateAsync()).ReturnsAsync(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity())));

            var component = testHost.AddComponent<NotAuthorizedHandler>();

            var nav = _testNavigationManager.Navigations.Pop();
            nav.uri.ShouldEndWith($"?returnUrl={WebUtility.UrlEncode("/test/test")}");
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
