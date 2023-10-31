using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Adicionar serviços ao contêiner de injeção de dependência.
builder.Services.AddRazorPages();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login"; // Especifica a página de login
        options.LogoutPath = "/Logout"; // Especifica a página de logout
        options.AccessDeniedPath = "/AcessoNegado"; // Especifica a página de acesso negado
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

app.UseAuthentication(); // Habilita o middleware de autenticação
app.UseAuthorization(); // Habilita o middleware de autorização

app.MapRazorPages();

app.Run();
