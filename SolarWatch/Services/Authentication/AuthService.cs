using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace SolarWatch.Services.Authentication;

public class AuthService : IAuthService
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly ITokenService _tokenService;

    public AuthService(UserManager<IdentityUser> userManager, ITokenService tokenService)
    {
        _userManager = userManager;
        _tokenService = tokenService;
    }
    
    public async Task<AuthResult> RegisterAsync(string email, string username, string password)
    {
        var result = await _userManager.CreateAsync(new IdentityUser { UserName = username, Email = email }, password);

        if (!result.Succeeded)
        {
            return FailedRegistration(result, email, username);
        }

        return new AuthResult(true, email, username, "");
    }

    public async Task<AuthResult> LoginAsync(string email, string password)
    {
        var managedUser = await _userManager.FindByEmailAsync(email);
        if (managedUser is null)
        {
            return InvalidEmail(email);
        }

        var isPasswordValid = await _userManager.CheckPasswordAsync(managedUser, password);
        if (!isPasswordValid)
        {
            return InvalidPassword(email, managedUser.UserName);
        }

        var accessTokens = _tokenService.CreateToken(managedUser);
        return new AuthResult(true, managedUser.Email, managedUser.UserName, accessTokens);
    }

    private static AuthResult FailedRegistration(IdentityResult result, string email, string username)
    {
        var authResult = new AuthResult(false, email, username, "");

        foreach (var error in result.Errors)
        {
            authResult.ErrorMessages.Add(error.Code, error.Description);
        }

        return authResult;
    }

    private static AuthResult InvalidEmail(string email)
    {
        var result = new AuthResult(false, email, "", "");
        result.ErrorMessages.Add("Bad credentials", "Invalid email");
        return result;
    }

    private static AuthResult InvalidPassword(string email, string username)
    {
        var result = new AuthResult(false, email, username, "");
        result.ErrorMessages.Add("Bad credentials", "Invalid password");
        return result;
    }
}