using CopyRecipeBookMVC.Application.Mapping;
using CopyRecipeBookMVC.Domain.Interfaces;
using CopyRecipeBookMVC.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using CopyRecipeBookMVC.Application;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using FluentValidation.AspNetCore;
using FluentValidation;
using CopyRecipeBookMVC.Application.ViewModels.Recipe;
using CopyRecipeBookMVC.Application.ViewModels.Ingredient;
using Microsoft.Extensions.Logging;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration
    .GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Configuration.AddJsonFile("katalog_z_kluczami/klucze.json", optional: true, reloadOnChange: true);
var googleApiKey = builder.Configuration["GoogleApiKey"];
builder.Services.AddDbContext<Context>(options =>
	options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => 
options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<CopyRecipeBookMVC.Infrastructure.Context>();

//builder.Services.ConfigureApplicationCookie(options =>
//{
//    options.Cookie.HttpOnly = true; // Ciasteczko nie dostêpne przez JavaScript
//    options.ExpireTimeSpan = TimeSpan.FromMinutes(60); // Czas ¿ycia ciasteczka
//    options.SlidingExpiration = true; // Przed³u¿enie sesji przy ka¿dej aktywnoœci
//    options.Cookie.SameSite = SameSiteMode.Lax; // Zapewnia prawid³ow¹ obs³ugê przy ¿¹daniach AJAX
//});

//builder.Services.AddSession(options =>
//{
//	options.IdleTimeout = TimeSpan.FromMinutes(30); // Czas trwania sesji  
//	options.Cookie.HttpOnly = true; // Zwiêksza bezpieczeñstwo  
//	options.Cookie.IsEssential = true; // Wymaga ciasteczka do dzia³ania  
//});


builder.Services.AddAplication();
builder.Services.AddInfrastructure();

builder.Services.AddControllersWithViews().AddViewOptions(opt => { opt.ClientModelValidatorProviders.Clear(); });

builder.Services.AddAuthentication()
	//.AddCookie()
    .AddGoogle(options =>
    {
        var googleAuthNSection = builder.Configuration.GetSection("Authentication:Google");
        options.ClientId = googleAuthNSection["ClientId"];
        options.ClientSecret = googleAuthNSection["ClientSecret"];
    });

builder.Services.AddFluentValidationAutoValidation(fv => 
fv.DisableDataAnnotationsValidation = true).AddFluentValidationClientsideAdapters();

builder.Services.AddTransient<IValidator<NewRecipeVm>, NewRecipeValidation>();
builder.Services.AddTransient<IValidator<IngredientForNewRecipeVm>, IngredientForNewRecipeValidation>();

builder.Logging.AddFile("Logs/myLog-{Date}.txt");


var app = builder.Build();

var defaultCulture = new CultureInfo("en-US");
var localizationOptions = new RequestLocalizationOptions
{
	DefaultRequestCulture = new RequestCulture(defaultCulture),
	SupportedCultures = new List<CultureInfo> { defaultCulture },
	SupportedUICultures = new List<CultureInfo> { defaultCulture }
};

app.UseRequestLocalization(localizationOptions);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseMigrationsEndPoint();
}
else
{
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
//app.UseSession();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
