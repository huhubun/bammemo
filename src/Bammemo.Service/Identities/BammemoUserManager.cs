using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text;

namespace Bammemo.Service.Identities;

public class BammemoUserManager(IUserStore<BammemoUser> store,
    IOptions<IdentityOptions> optionsAccessor,
    IPasswordHasher<BammemoUser> passwordHasher,
    IEnumerable<IUserValidator<BammemoUser>> userValidators,
    IEnumerable<IPasswordValidator<BammemoUser>> passwordValidators,
    ILookupNormalizer keyNormalizer,
    IdentityErrorDescriber errors,
    IServiceProvider services,
    ILogger<UserManager<BammemoUser>> logger) : UserManager<BammemoUser>(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
{
    public override Task<bool> CheckPasswordAsync(BammemoUser user, string password)
        => Task.FromResult(String.Equals(user.Password, Convert.ToBase64String(Encoding.UTF8.GetBytes(password))));
}
