using Microsoft.AspNetCore.Identity;
using System.Collections.ObjectModel;

namespace Bammemo.Service.Identities
{
    public class BammemoUserStore : IUserStore<BammemoUser>
    {
        private static ReadOnlyCollection<BammemoUser> Users => new(
        [
            new BammemoUser(SafeGetEnvironmentVariable("BAMMEMO_USERNAME"), SafeGetEnvironmentVariable("BAMMEMO_PASSWORD"))
        ]);

        public Task<IdentityResult> CreateAsync(BammemoUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityResult> DeleteAsync(BammemoUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
        }

        public Task<BammemoUser?> FindByIdAsync(string userId, CancellationToken cancellationToken)
            => Task.FromResult(Users.SingleOrDefault(u => String.Equals(u.Id, userId, StringComparison.OrdinalIgnoreCase)));

        public Task<BammemoUser?> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
            => Task.FromResult(Users.SingleOrDefault(u => String.Equals(u.Username, normalizedUserName, StringComparison.OrdinalIgnoreCase)));

        public Task<string?> GetNormalizedUserNameAsync(BammemoUser user, CancellationToken cancellationToken)
            => Task.FromResult<string?>(user.Username);

        public Task<string> GetUserIdAsync(BammemoUser user, CancellationToken cancellationToken)
            => Task.FromResult(user.Id);

        public Task<string?> GetUserNameAsync(BammemoUser user, CancellationToken cancellationToken)
            => Task.FromResult<string?>(user.Username);

        public Task SetNormalizedUserNameAsync(BammemoUser user, string? normalizedName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetUserNameAsync(BammemoUser user, string? userName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityResult> UpdateAsync(BammemoUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        private static string SafeGetEnvironmentVariable(string variable)
            => Environment.GetEnvironmentVariable(variable) ?? throw new NullReferenceException($"Environment variable '{variable}' not set.");
    }
}
