using SolarWatch.Services.Authentication;

namespace SolarWatch.IntegrationTests.TestServices;

public class TestAuthServices : IAuthService
{
    public Task<AuthResult> RegisterAsync(string email, string username, string password, string role)
    {
        return Task.FromResult(new AuthResult(true, "", "", ""));
    }

    public Task<AuthResult> LoginAsync(string email, string password)
    {
        return Task.FromResult(new AuthResult(true, "", "", ""));
    }
}