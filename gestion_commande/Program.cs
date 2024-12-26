using gestion_commande.Data;
using gestion_commande.Services;
using gestion_commande.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Ajouter les contrôleurs avec les vues
builder.Services.AddControllersWithViews();

// Configurer le DbContext avec la chaîne de connexion
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Ajouter les services au conteneur d'injection de dépendances
builder.Services.AddScoped<IClientService, ClientService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICommandeService, CommandeService>();
builder.Services.AddScoped<IDetailsService, DetailsService>();
builder.Services.AddScoped<IPaiementService, PaiementService>();
builder.Services.AddScoped<IProduitService, ProduitService>();

// Ajouter IHttpContextAccessor pour l'accès au contexte HTTP
builder.Services.AddHttpContextAccessor();

// Configurer l'authentification par cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Authentification/Login"; // Page de connexion
        options.AccessDeniedPath = "/Authentification/AccessDenied"; // Page d'accès refusé
    });


var app = builder.Build();

// Gérer les erreurs pour les environnements autres que le développement
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// Middleware pour la redirection HTTPS
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// Ajouter l'authentification et l'autorisation
app.UseAuthentication();
app.UseAuthorization();

// Configurer les routes des contrôleurs
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Authentification}/{action=Login}/{id?}");

// Lancer l'application
app.Run();
