using Microsoft.AspNetCore.Identity;

namespace HotelPrenotazioni.Models;

public class ApplicationUser : IdentityUser
{
    public string NomeCompleto { get; set; } = string.Empty;
}
