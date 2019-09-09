using BlazorAuthenticationSample.Client.Features.App.Components;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Testing;
using System;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Components.Authorization;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components.Rendering;

namespace BlazorAuthenticationSample.Client.Tests
{

    public class SidebarTests
    {
        private TestHost testHost = new TestHost();

        public SidebarTests()
        {

            testHost.ConfigureServices(services =>
            {
                services.AddLogging();
                services.AddAuthorization(options =>
                {
                    options.AddPolicy("RequireAdmin", c => c.RequireRole("Admin"));
                });
            });
        }

        [Fact]
        public void ShouldShowAdminMenuIfUserHasAdminRole()
        {
            var principal = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Role, "Admin") }));

            Task<AuthenticationState> t = Task.FromResult(new AuthenticationState(principal));

            var parameterView = ParameterView.FromDictionary(new Dictionary<string, object> { ["AuthenticationState"] = t });

            var sidebar = testHost.AddComponent<SidebarWrapperTestComponent>(parameterView);

            var menuLabel = sidebar.Find("#admin-menu-label");

            Assert.NotNull(menuLabel);
        }

        [Fact]
        public void ShouldNotShowAdminMenuIfUserDoesNotHaveAdminRole()
        {
            var principal = new ClaimsPrincipal(new ClaimsIdentity());

            Task<AuthenticationState> t = Task.FromResult(new AuthenticationState(principal));

            var parameterView = ParameterView.FromDictionary(new Dictionary<string, object> { ["AuthenticationState"] = t });

            var sidebar = testHost.AddComponent<SidebarWrapperTestComponent>(parameterView);

            var menuLabel = sidebar.Find("#admin-menu-label");

            Assert.Null(menuLabel);
        }

        public class SidebarWrapperTestComponent : ComponentBase
        {
            [Parameter] public Task<AuthenticationState> AuthenticationState { get; set; }

            protected override void BuildRenderTree(RenderTreeBuilder builder)
            {
                builder.OpenComponent<CascadingValue<Task<AuthenticationState>>>(0);
                builder.AddAttribute(1, nameof(CascadingValue<Task<AuthenticationState>>.Value), AuthenticationState);
                builder.AddAttribute(2, "ChildContent", (RenderFragment)(builder =>
                {
                    builder.OpenComponent<Sidebar>(0);
                    builder.CloseComponent();
                }));
                builder.CloseComponent();
            }
        }
    }
}
