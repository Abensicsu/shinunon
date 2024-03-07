using DataModels.Data;
using DataModels.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ShWeb.Components;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddControllers();
builder.Services.AddRazorPages();

builder.Services.AddScoped<PlanExamService>();

builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri("https://localhost:7115")
});

builder.Services.AddDbContext<SHcx>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("PGConnection"), x => x.MigrationsAssembly("ShWeb"));
    options.UseSnakeCaseNamingConvention();
#if DEBUG
    options.EnableSensitiveDataLogging();
#endif
    //options.("ShWeb");
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
