# ASP.NET Core Blazor (Server side) Authentication Sample
This sample shows some more "complex" and "real world" scenarios for handling `Authentication` and `Authorization` with ASP.NET Core server side Blazor.

The idea was to organize the sample a bit more like a real app, and just go a tiny step further than the built in template that comes in the box.

It's a work in progress... 🤷‍♂️

## Structure
The app is comprised of few "parts". The `App` part and the `Account` part, plus a few other things. 

### Account part

The `Account` part contains pages to register and and sign in etc. 

A little note is the `SignIn` page where there is a little extra hacky step as you cannot set cookies from Blazor easily. See for yourself 🤣

### App part

In the `App` you need to be authenticated. This was done by adding `@attribute [Authorize]` to `_Imports.razor` in the `App/Pages` so all pages in that folder require authorization.

There is also a `Sidebar` menu that uses an `AuthorizeView` to trigger and "administrator" section of the menu based on roles.

## Data
When started the app sets up some test users, see `Startup.cs` for those.

# Unit tests
There is a few tests of some of the custom components as I also wanted to explore Unit Testing of Blazor Components.

The tests are using a slightly modified version of Steve Sanderson's [BlazorUnitTestingPrototype](https://github.com/SteveSandersonMS/BlazorUnitTestingPrototype). See [my fork here](https://github.com/christiansparre/BlazorUnitTestingPrototype/tree/feature/testhost). Also see Steve's [blog post](http://blog.stevensanderson.com/2019/08/29/blazor-unit-testing-prototype/) about the testing prototype.
 
# Ideas

- Would like to see if one could make a component to "generalize" some of the code around [Resource-based authorization](https://docs.microsoft.com/en-us/aspnet/core/security/authorization/resourcebased)
- Explore some more testing, I think Steve's prototype is a nice foundation