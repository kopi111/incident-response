using IncidentResponse.Auth;
using IncidentResponse.Data;
using IncidentResponse.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MudBlazor.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("http://localhost:5100");

builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseSqlite("Data Source=incidents.db"));

builder.Services.AddScoped<IIncidentService, IncidentService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IMetricsService, MetricsService>();
builder.Services.AddScoped<ICaseService, CaseService>();
builder.Services.AddScoped<IAlertService, AlertService>();

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddMudServices();

builder.Services.AddControllers();

var jwtKey = builder.Configuration["Jwt:Key"] ?? "incident-response-super-secret-jwt-key-2024-secure";
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = "multi";
    options.DefaultChallengeScheme = "multi";
})
.AddPolicyScheme("multi", "multi", options =>
{
    options.ForwardDefaultSelector = ctx =>
    {
        if (ctx.Request.Path.StartsWithSegments("/api"))
            return JwtBearerDefaults.AuthenticationScheme;
        return "Cookies";
    };
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
        ValidateIssuer = false,
        ValidateAudience = false
    };
})
.AddCookie("Cookies", options =>
{
    options.LoginPath = "/login";
    options.ExpireTimeSpan = TimeSpan.FromHours(8);
});

builder.Services.AddAuthorization();

builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
    await DataSeeder.SeedAsync(db);
}

app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();
app.MapControllers();
app.MapRazorComponents<IncidentResponse.Components.App>()
    .AddInteractiveServerRenderMode();

app.Run();
