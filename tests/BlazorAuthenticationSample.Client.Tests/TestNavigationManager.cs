using Microsoft.AspNetCore.Components;
using System.Collections.Generic;

namespace BlazorAuthenticationSample.Client.Tests
{
    public class TestNavigationManager : NavigationManager
    {
        public Stack<(string uri, bool forceLoad)> Navigations { get; set; } = new Stack<(string uri, bool forceLoad)>();

        public void SetInitialized(string baseUri = "https://example.com/", string uri = "test/test") => Initialize(baseUri, baseUri + uri);

        protected override void NavigateToCore(string uri, bool forceLoad)
        {
            Navigations.Push((uri, forceLoad));
        }
    }
}
