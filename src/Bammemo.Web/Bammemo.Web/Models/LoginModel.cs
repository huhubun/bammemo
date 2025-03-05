using System.ComponentModel.DataAnnotations;

namespace Bammemo.Web.Models;

public class LoginModel
{
    [Required]
    public string? Username { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string? Password { get; set; }
}
