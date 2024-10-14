using DataModels.Data;
using DataModels.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ShWeb.Components;
using ShWeb.Components.BAServices;
using System.Reflection;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using Newtonsoft.Json.Serialization;
using DataModels.Utilities;
using Newtonsoft.Json;
using System.Runtime.Serialization;

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

//.AddJsonOptions(options =>
// {
//     options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
//     options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;

//     options.JsonSerializerOptions.TypeInfoResolver = new DefaultJsonTypeInfoResolver
//     {
//         Modifiers =
//            {
//                static jsonTypeInfo =>
//                {
//                    // Check if the type is abstract or an interface
//                    if (jsonTypeInfo.Type.IsAbstract || jsonTypeInfo.Type.IsInterface)
//                    {
//                        var polymorphismOptions = new JsonPolymorphismOptions
//                        {
//                            TypeDiscriminatorPropertyName = "$type",
//                            UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FallBackToBaseType
//                        };

//                        // Automatically add all derived types that are not abstract
//                        var derivedTypes = Assembly.GetExecutingAssembly()
//                            .GetTypes()
//                            .Where(t => jsonTypeInfo.Type.IsAssignableFrom(t) && !t.IsAbstract)
//                            .Select(t => new JsonDerivedType(t, t.Name));

//                        foreach (var derivedType in derivedTypes)
//                        {
//                            polymorphismOptions.DerivedTypes.Add(derivedType);
//                        }

//                        jsonTypeInfo.PolymorphismOptions = polymorphismOptions;
//                    }
//                }
//            }
//     };
// });



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
