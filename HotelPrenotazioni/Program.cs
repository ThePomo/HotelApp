using HotelPrenotazioni.Data;
using HotelPrenotazioni.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Connessione DB
builder.Services.AddDbContext<HotelDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configurazione di Identity con utenti e ruoli personalizzati
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    

    // Imposta la lunghezza minima della password
    options.Password.RequiredLength = builder.Configuration.GetValue<int>("Identity:RequiredLength");

    // Richiede che la password contenga almeno un numero
    options.Password.RequireDigit = builder.Configuration.GetValue<bool>("Identity:RequireDigit");

    // Richiede almeno una lettera minuscola nella password
    options.Password.RequireLowercase = builder.Configuration.GetValue<bool>("Identity:RequireLowercase");

    // Richiede almeno un carattere speciale nella password
    options.Password.RequireNonAlphanumeric = builder.Configuration.GetValue<bool>("Identity:RequireNonAlphanumeric");

    // Richiede almeno una lettera maiuscola nella password
    options.Password.RequireUppercase = builder.Configuration.GetValue<bool>("Identity:RequireUppercase");
})
    // Utilizza il contesto del database per archiviare utenti e ruoli
    .AddEntityFrameworkStores<HotelDbContext>()
    .AddDefaultTokenProviders();

// Configurazione dell'autenticazione con i cookie
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login"; // Percorso della pagina di login
        options.AccessDeniedPath = "/Account/Login"; // Pagina di accesso negato
        options.Cookie.HttpOnly = true; // Impedisce l'accesso ai cookie tramite JavaScript per motivi di sicurezza
        options.Cookie.Name = "EcommerceLiveEfCore"; // Nome del cookie per l'autenticazione
    });

// MVC + Razor Pages 
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

// Middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication(); // Aggiungi questo per abilitare l'autenticazione
app.UseAuthorization();

// Route predefinita
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages(); // Aggiungi questa sezione per le pagine Razor

// Inizializza database e ruoli
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await DbInitializer.InitializeAsync(services); // Assicurati che questa funzione inizializzi correttamente i ruoli
}

app.Run();
