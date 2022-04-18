using System.Security.Claims;
using System.Threading.Tasks;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Validation;
using Identity.Identity.Models;
using Microsoft.AspNetCore.Identity;

namespace Identity;

public class UserValidator : IResourceOwnerPasswordValidator
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;

    public UserValidator(SignInManager<ApplicationUser> signInManager,
        UserManager<ApplicationUser> userManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
    }

    public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
    {
        var user = await _userManager.FindByNameAsync(context.UserName);

        var signIn = await _signInManager.PasswordSignInAsync(user,
            context.Password, true, lockoutOnFailure: true);

        if (signIn.Succeeded)
        {
            // context set to success
            context.Result = new GrantValidationResult(
                subject: user?.Id.ToString(),
                authenticationMethod: "custom",
                claims: new Claim[] { new Claim(ClaimTypes.NameIdentifier, user!.UserName) }
            );

            return;
        }

        // context set to Failure
        context.Result = new GrantValidationResult(
            TokenRequestErrors.UnauthorizedClient, "Invalid Crdentials");
    }
}
