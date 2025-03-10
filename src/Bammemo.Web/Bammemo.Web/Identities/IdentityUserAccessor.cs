using Bammemo.Service.Identities;
using Microsoft.AspNetCore.Identity;

namespace Bammemo.Web.Identities;

internal sealed class IdentityUserAccessor(UserManager<BammemoUser> userManager, IdentityRedirectManager redirectManager)
{
    public async Task<BammemoUser> GetRequiredUserAsync(HttpContext context)
    {
        var user = await userManager.GetUserAsync(context.User);

        if (user is null)
        {
            redirectManager.RedirectToWithStatus("Account/InvalidUser", $"Error: Unable to load user with ID '{userManager.GetUserId(context.User)}'.", context);
        }

        return user;
    }
}
