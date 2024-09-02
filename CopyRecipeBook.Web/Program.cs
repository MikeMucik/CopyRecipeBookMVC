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

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<Context>(options =>
	options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
	.AddEntityFrameworkStores<Context>();

builder.Services.AddAplication();
builder.Services.AddInfrastructure();

builder.Services.AddControllersWithViews().AddViewOptions(opt => { opt.ClientModelValidatorProviders.Clear(); });

builder.Services.AddFluentValidationAutoValidation(fv => fv.DisableDataAnnotationsValidation = true).AddFluentValidationClientsideAdapters();

builder.Services.AddTransient<IValidator<NewRecipeVm>, NewRecipeValidation>();
//builder.Services.AddTransient<IValidator<IngredientForNewRecipeVm>, IngredientForNewRecipeValidation>();
// usuniête bo mam tylko imitacjê fluent validation
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

app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
