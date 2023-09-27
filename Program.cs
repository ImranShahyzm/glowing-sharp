using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using SagErpBlazor.Data;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Configuration;
using SagErpBlazor.Services;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Blazored.LocalStorage;
using SagErpBlazor.Models;
using Microsoft.EntityFrameworkCore;
using SagErpBlazor.ModelServices;
using Blazorise;
using Blazorise.Bootstrap;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Tewr.Blazor.FileReader;
using SagErpBlazor.ExtendedClasses;
using Microsoft.AspNetCore.SignalR.Client;
using SagErpBlazor.SignalRHub;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor().AddHubOptions(o =>
{
    o.MaximumReceiveMessageSize = 10 * 1024 * 1024;
});
builder.Services.AddSingleton<WeatherForecastService>();

builder.Services.AddSingleton(builder.Configuration.GetSection("MailSettings").Get<MailSettings>());
builder.Services.AddScoped<DataBaseServices>();
builder.Services.AddScoped<CompanyService>();
builder.Services.AddScoped<StsProjectRegisterService>();
builder.Services.AddScoped<StsCustomerRegisterService>();
builder.Services.AddScoped<ImageService>();
builder.Services.AddScoped<IMailInterface, MailService>();
IControllerService.RegisterServices(builder.Services);

builder.Services.AddBlazorise()
      .AddBootstrapProviders();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddHttpClient<IUserService, UserService>();

builder.Services.AddSingleton<HttpClient>();


builder.Services.AddDbContext<BlazorSupportTicketsContext>(options =>
                       options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionStringName")));

builder.Services.Configure<ApiSettings>(builder.Configuration.GetSection("ApiSettings"));

builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.CheckConsentNeeded = context => true;
    options.MinimumSameSitePolicy = SameSiteMode.None;
});
builder.Services.AddAuthentication(
    CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie();

// Add authorization with default policy requiring authenticated users.
builder.Services.AddAuthorization(options =>
{
    options.DefaultPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});

 builder.Services.AddHttpContextAccessor();
 builder.Services.AddScoped<HttpContextAccessor>();
 builder.Services.AddHttpClient();
 builder.Services.AddScoped<HttpClient>();
 builder.Services.AddScoped<SessionService>();
//****** Implementing for Protection ************//

builder.Services.AddDataProtection()
     .SetApplicationName("Support Managment System");


        var jwtSection = builder.Configuration.GetSection("JWTSettings");
        builder.Services.Configure<JWTSettings>(jwtSection);
        var appSettings = jwtSection.Get<JWTSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.SecretKey);
builder.Services.AddSignalR();
builder.Services.AddScoped<HubConnection>(provider =>
{
    var hubConnection = new HubConnectionBuilder()
       .WithUrl("https://localhost:7272/TicketsHub")// Update the URL to match your Hub URL
        .Build();

    return hubConnection;
});
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = true;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false,
        ClockSkew = TimeSpan.Zero
    };
});



//builder.Services.AddServerSideBlazor().AddHubOptions(o =>
//{
//    o.MaximumReceiveMessageSize = 10 * 1024 * 1024;
//});

//*******************************************//
//builder.Services.AddFileReaderService();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCookiePolicy();

app.UseDeveloperExceptionPage();
app.UseAuthentication();

app.UseRouting();
app.UseAuthorization();
app.MapHub<TicketsHub>("Ticketshub");
app.MapControllers();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");



app.Run();
