using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;

namespace Bammemo.Service.Options;

public class BammemoOptions
{
    public const string Position = "Bammemo";

    [Required]
    public required string ConnectionString { get; set; }

    [Required]
    public required string ApiUrl { get; set; }

    [Required]
    public required string Username { get; set; }

    [Required]
    public required string Password { get; set; }

    [Required]
    public required string StoragePath { get; set; }

    public string? Key { get; set; }
}
