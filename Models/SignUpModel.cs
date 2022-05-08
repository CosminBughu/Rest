using System.ComponentModel.DataAnnotations;

namespace Rest.Models;

public class SignUpModel
{
    [Required]
    public string UserName { get; set; }
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    [Compare("ConfirmPassword")]
    public string Password { get; set; }
    [Required]
    public string ConfirmPassword { get; set; }
}
