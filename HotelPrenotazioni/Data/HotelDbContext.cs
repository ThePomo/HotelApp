using HotelPrenotazioni.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HotelPrenotazioni.Data;

public class HotelDbContext : IdentityDbContext<ApplicationUser>
{
    public HotelDbContext(DbContextOptions<HotelDbContext> options)
        : base(options) { }

    public DbSet<Cliente> Clienti { get; set; }
    public DbSet<Camera> Camere { get; set; }
    public DbSet<Prenotazione> Prenotazioni { get; set; }
}
