using System.Diagnostics.CodeAnalysis;

namespace Bammemo.Service.Identities;

public class BammemoUser
{
    public BammemoUser()
    {
    }

    [SetsRequiredMembers]
    public BammemoUser(string username, string password)
    {
        Id = username;
        Username = username;
        Password = password;
    }

    public required string Id { get; set; }
    public required string Username { get; set; }
    public required string Password { get; set; }
}
