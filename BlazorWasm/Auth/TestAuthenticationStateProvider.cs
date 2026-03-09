using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace BlazorWasm.Auth;

public class TestAuthenticationStateProvider : AuthenticationStateProvider
{
    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        // Define a fake test user
        var identity = new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.Name, "TestUser"),
            new Claim(ClaimTypes.Email, "testuser@avrang.tj")
        }, "Fake Authentication Type");

        var user = new ClaimsPrincipal(identity);
        return Task.FromResult(new AuthenticationState(user));
    }
}
