using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Adicionar servi�os ao cont�iner de inje��o de depend�ncia.
builder.Services.AddRazorPages();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login"; // Especifica a p�gina de login
        options.LogoutPath = "/Logout"; // Especifica a p�gina de logout
        options.AccessDeniedPath = "/AcessoNegado"; // Especifica a p�gina de acesso negado
    });

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication(); // Habilita o middleware de autentica��o
app.UseAuthorization(); // Habilita o middleware de autoriza��o

app.MapRazorPages();

app.Run();
