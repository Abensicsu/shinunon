using DataModels.Data;
using DataModels.Services;
using Microsoft.EntityFrameworkCore;
using ShWeb.Components;
using ShWeb.Components.BAServices;
using DataModels.Utilities;
using Newtonsoft.Json;
using DataModels.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddControllers()
        .AddNewtonsoftJson(options =>
        {
            options.SerializerSettings.TypeNameHandling = TypeNameHandling.Auto;
            options.SerializerSettings.TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple;
            options.SerializerSettings.SerializationBinder = new KnownTypesBinder(JsonKnownTypes.KnownTypes);
            options.SerializerSettings.PreserveReferencesHandling = PreserveReferencesHandling.Objects;
            options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
        });

builder.Services.AddRazorPages();

builder.Services.AddRazorPages();

builder.Services.AddScoped<ExamPlanService>();
builder.Services.AddScoped<ExamExecutionService>();
builder.Services.AddScoped<SefariaService>();

builder.Services.AddScoped(sp => new HttpClient
{
#if DEBUG
    BaseAddress = new Uri("https://localhost:7115")
#else
    BaseAddress = new Uri("https://shn.datasyspro.com")
#endif
});

builder.Services.AddScoped<CustomHttpClientService>();
builder.Services.AddScoped<LocalStorageService>();
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();

builder.Services.AddDbContext<SHcx>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("PGConnection"), x => x.MigrationsAssembly("ShWeb"));
    options.UseSnakeCaseNamingConvention();
#if DEBUG
    options.EnableSensitiveDataLogging();
#endif
    //options.("ShWeb");
});

builder.Services.AddDefaultIdentity<User>(options =>
{

    options.User.AllowedUserNameCharacters = null; // Allow any characters

    options.User.RequireUniqueEmail = true; 
    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+"; //white list of chars in user-name

    options.SignIn.RequireConfirmedAccount = false; // Users are not required to confirm their account via email.

    // Password requirements
    options.Password.RequireDigit = true; // At least one numeric digit (e.g., 0-9).
    options.Password.RequiredLength = 6; // At least 8 characters long.
    options.Password.RequireNonAlphanumeric = false; 
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false; 
})
.AddEntityFrameworkStores<SHcx>();



builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    object Configuration = null;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidAudience = builder.Configuration["JWT:ValidAudience"],
        ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration["JWT:SecretKey"]))
    };
});
var app = builder.Build();

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();
app.MapControllers();
app.Run();
