using Digital_Wallet.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//Database Integration
builder.Services.AddDbContext<ApplicationDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("MyConnection")));

//Register Syncfusion license
Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1NBaF5cWWFCf1FpRmJGdld5fUVHYVZUTXxaS00DNHVRdkdnWXpfdnVWQmheWERzXEE=");

//authentcation and session 

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
.AddCookie(option =>
 {
     option.LoginPath = "/Access/Login";
     option.ExpireTimeSpan = TimeSpan.FromMinutes(20);
 });
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("OnlySpecificUserPolicy", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireClaim(ClaimTypes.Email, "Gnsh@admin.com"); // Make sure this matches with your actual username
    });
});

/*
//Session
builder.Services.AddSession(
    options => {
        options.IdleTimeout = TimeSpan.FromSeconds(1000);
        options.Cookie.HttpOnly=true;
        options.Cookie.IsEssential = true;
    }
);
*/
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Access}/{action=Login}/{id?}");
//app.UseSession();
app.Run();
