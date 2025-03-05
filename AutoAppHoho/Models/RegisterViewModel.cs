using System.ComponentModel.DataAnnotations;

public class RegisterViewModel
{
    [Required]
    public string Voornaam { get; set; }

    [Required]
    public string Achternaam { get; set; }

    [Required]
    public string UserName { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [Phone]
    public string PhoneNumber { get; set; } 

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "De wachtwoorden komen niet overeen.")]
    public string ConfirmPassword { get; set; }
}
