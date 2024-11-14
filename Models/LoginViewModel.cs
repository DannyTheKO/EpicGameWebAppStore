using System.ComponentModel.DataAnnotations;

namespace EpicGameWebAppStore.Models;

public class LoginViewModel
{
    [Required(ErrorMessage = "Username is required to login!")]
    public string Username { get; set; }

    [Required(ErrorMessage = "Password is required to login!")]
    public string Password { get; set; }
}