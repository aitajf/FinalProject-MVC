using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Service;

var builder = WebApplication.CreateBuilder(args);

// Authentication
builder.Services.AddAuthentication("MyCookieAuth")
    .AddCookie("MyCookieAuth", options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";
    });

builder.Services.AddAuthorization();

// Session
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(15);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// MVC və digər servis
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();
builder.Services.Configure<IdentityOptions>(opt =>
{
    opt.User.RequireUniqueEmail = true;
    opt.Password.RequiredUniqueChars = 1;
    opt.Password.RequireNonAlphanumeric = true;
    opt.Password.RequireDigit = true;
    opt.Password.RequireLowercase = true;
    opt.Password.RequireUppercase = true;
    opt.SignIn.RequireConfirmedEmail = true;
});
builder.Services.AddServiceLayer();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// ✅ Session əvvəl gəlməlidir
app.UseSession();

// ✅ Routing əvvəl middleware
app.UseRouting();

// ✅ Session'dan User yükləmə middleware
app.Use(async (context, next) =>
{
    var token = context.Session.GetString("AuthToken");
    if (!string.IsNullOrEmpty(token))
    {
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);

        var claims = jwtToken.Claims.Select(c =>
        {
            if (c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")
                return new Claim(ClaimTypes.NameIdentifier, c.Value);

            if (c.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role")
                return new Claim(ClaimTypes.Role, c.Value);

            return new Claim(c.Type, c.Value);
        });

        var identity = new ClaimsIdentity(claims, "jwt");
        var principal = new ClaimsPrincipal(identity);
        context.User = principal;
    }

    await next();
});




// ✅ Yalnız bir dəfə çağır
app.UseAuthentication();
app.UseAuthorization();

// Routes
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
