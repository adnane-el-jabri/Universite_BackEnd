using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using UniversiteDomain.DataAdapters;
using UniversiteDomain.JeuxDeDonnees;
using UniversiteDomain.UseCases.EtudiantUseCases.Create;
using UniversiteDomain.UseCases.EtudiantUseCases.Delete;
using UniversiteDomain.UseCases.EtudiantUseCases.Update;
using UniversiteDomain.UseCases.EtudiantUseCases.Get;
using UniversiteEFDataProvider.Data;
using UniversiteEFDataProvider.Entities;
using UniversiteEFDataProvider.RepositoryFactories;
using UniversiteEFDataProvider.Repositories;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Mvc;
using UniversiteDomain.UseCases;
using UniversiteDomain.UseCases.ParcoursUseCases.Delete;
using UniversiteDomain.UseCases.ParcoursUseCases.Get;
using UniversiteDomain.UseCases.ParcoursUseCases.Update;
using UniversiteDomain.UseCases.UeUseCases.Create;
using UniversiteDomain.UseCases.UeUseCases.Delete;
using UniversiteDomain.UseCases.UeUseCases.Get;
using UniversiteDomain.UseCases.UeUseCases.Update;

var builder = WebApplication.CreateBuilder(args);

// Ajout des services essentiels
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddLogging(options =>
{
    options.ClearProviders();
    options.AddConsole();
});

// Configuration de la connexion à MySql
string connectionString = builder.Configuration.GetConnectionString("MySqlConnection") 
    ?? throw new InvalidOperationException("Connection string 'MySqlConnection' not found.");

builder.Services.AddDbContext<UniversiteDbContext>(options => options.UseMySQL(connectionString));

// Enregistrement des services de la BD et des Repositories
builder.Services.AddScoped<IRepositoryFactory, RepositoryFactory>();
builder.Services.AddScoped<IEtudiantRepository, EtudiantRepository>();
builder.Services.AddScoped<IUeRepository, UeRepository>();
builder.Services.AddScoped<IParcoursRepository, ParcoursRepository>();
builder.Services.AddScoped<INoteRepository, NoteRepository>();

builder.Services.AddScoped<CreateEtudiantUseCase>();
builder.Services.AddScoped<DeleteEtudiantUseCase>();
builder.Services.AddScoped<UpdateEtudiantUseCase>();
builder.Services.AddScoped<GetEtudiantCompletUseCase>();
// Enregistrement des UseCases pour Parcours
builder.Services.AddScoped<CreateParcoursUseCase>();  // Ajout de cette ligne
builder.Services.AddScoped<DeleteParcoursUseCase>();
builder.Services.AddScoped<UpdateParcoursUseCase>();
builder.Services.AddScoped<GetParcoursUseCase>();
// Enregistrement des UseCases pour UE
builder.Services.AddScoped<CreateUeUseCase>();
builder.Services.AddScoped<GetUeUseCase>();
builder.Services.AddScoped<UpdateUeUseCase>();
builder.Services.AddScoped<DeleteUeUseCase>();

builder.Services.AddScoped<GenerateCsvForUeNotesUseCase>();
builder.Services.AddScoped<UploadCsvForUeNotesUseCase>();
builder.Services.AddScoped<ValidationUseCase>();

// Sécurisation : Configuration d’Identity
builder.Services.AddIdentity<UniversiteUser, UniversiteRole>()
    .AddEntityFrameworkStores<UniversiteDbContext>()
    .AddDefaultTokenProviders();

// Configuration de l'authentification JWT
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("VotreCléSecrèteTrèsLongueEtSûre")),
        ValidateIssuer = false,
        ValidateAudience = false,
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddAuthorization();

// Ajout des services pour UserManager, RoleManager, et IEmailSender
builder.Services.AddScoped<UserManager<UniversiteUser>>();
builder.Services.AddScoped<RoleManager<UniversiteRole>>();
builder.Services.AddSingleton<IEmailSender<UniversiteUser>, IdentityNoOpEmailSender>();

var app = builder.Build();

// Configuration du pipeline de requêtes
app.UseRouting();
app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// Ajout des routes d'identité personnalisées
// Ajout des routes d'identité personnalisées
app.MapPost("/custom-login", async ([FromBody] LoginDto loginDto, UserManager<UniversiteUser> userManager, RoleManager<UniversiteRole> roleManager) =>
{
    var user = await userManager.FindByEmailAsync(loginDto.Email);
    if (user != null && await userManager.CheckPasswordAsync(user, loginDto.Password))
    {
        // Récupérer les rôles de l'utilisateur
        var userRoles = await userManager.GetRolesAsync(user);
        var authClaims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        // Ajouter chaque rôle trouvé aux claims
        foreach (var role in userRoles)
        {
            authClaims.Add(new Claim(ClaimTypes.Role, role));
        }

        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("VotreCléSecrèteTrèsLongueEtSûre"));

        var token = new JwtSecurityToken(
            expires: DateTime.Now.AddHours(3),
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
        );

        return Results.Ok(new
        {
            token = new JwtSecurityTokenHandler().WriteToken(token),
            expiration = token.ValidTo
        });
    }
    return Results.Unauthorized();
});


// Ajout des routes d'identité prédéfinies
app.MapIdentityApi<UniversiteUser>();

// Initialisation de la base de données (uniquement en développement)
if (app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<UniversiteDbContext>>();
        var context = scope.ServiceProvider.GetRequiredService<UniversiteDbContext>();

        logger.LogInformation("Initialisation de la base de données");
        await context.Database.EnsureDeletedAsync();
        await context.Database.EnsureCreatedAsync();
    }
}

// Initialisation des données de test
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<UniversiteDbContext>();
    var repositoryFactory = scope.ServiceProvider.GetRequiredService<IRepositoryFactory>();

    var seedBD = new BasicBdBuilder(repositoryFactory);
    await seedBD.BuildUniversiteBdAsync();
}

// Démarrage de l'application
app.Run();

// Implémentation d'un email sender factice pour éviter les erreurs liées à IEmailSender
public class IdentityNoOpEmailSender : IEmailSender<UniversiteUser>
{
    public Task SendConfirmationLinkAsync(UniversiteUser user, string email, string confirmationLink) => Task.CompletedTask;
    public Task SendPasswordResetLinkAsync(UniversiteUser user, string email, string resetLink) => Task.CompletedTask;
    public Task SendPasswordResetCodeAsync(UniversiteUser user, string email, string resetCode) => Task.CompletedTask;
}

// DTO pour la connexion
public class LoginDto
{
    public string Email { get; set; }
    public string Password { get; set; }
}
