using AutoAppHoho.Data;
using AutoAppHoho.Models;
using AutoAppHoho.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using AutoAppHoho.Resources;
using Microsoft.Extensions.Localization;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;
    options.Password.RequiredLength = 8;
    options.Password.RequireDigit = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredUniqueChars = 1;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;
    options.User.RequireUniqueEmail = true;
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders()
.AddDefaultUI();

builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<IEmailSender, EmailSender>();

var jwtSettings = builder.Configuration.GetSection("Jwt");
var jwtKey = jwtSettings["Key"] ?? throw new InvalidOperationException("JWT Key missing.");

var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));

builder.Services.AddAuthentication()
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = signingKey,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"]
        };
    });

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "AutoAppHoho API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Voer 'Bearer {token}' in het invoerveld.",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            new string[] { }
        }
    });
});

builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

builder.Services.AddControllersWithViews()
    .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
    .AddDataAnnotationsLocalization();

var supportedCultures = new[]
{
    new CultureInfo("nl"),
    new CultureInfo("en"),
    new CultureInfo("fr")
};

var app = builder.Build(); 

var localizationOptions = new RequestLocalizationOptions()
    .SetDefaultCulture("nl")
    .AddSupportedCultures(supportedCultures.Select(c => c.Name).ToArray())
    .AddSupportedUICultures(supportedCultures.Select(c => c.Name).ToArray());

app.UseRequestLocalization(localizationOptions);


app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseRequestLocalization(localizationOptions);
app.UseAuthentication();
app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    await SeedData.Initialize(services, userManager, roleManager);
}

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();
app.MapControllers();
Language.InitLanguages();

app.Run();
