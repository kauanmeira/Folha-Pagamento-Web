using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Folha_Pagamento_Web.Models
{
    public class UsuarioModel : PageModel
    {
        [BindProperty]
        public int Id { get; set; }
        [BindProperty]
        public string Nome { get; set; }

        [BindProperty]
        public string Email { get; set; }

        [BindProperty]
        public string Senha { get; set; }

        [BindProperty]
        public int PermissaoId { get; set; }

        public List<UsuarioModel> Usuarios { get; set; }
        public string ApiResponseContent { get; set; }

        [TempData]
        public string? Token { get; set; }

        public UsuarioModel()
        {
            if (Token == null)
            {
                Token = ""; // Define um valor padrão vazio se o Token for nulo
            }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            // Recupere o token da TempData
            Token = TempData["Token"]?.ToString() ?? Token;

            if (string.IsNullOrEmpty(Token))
            {
                ModelState.AddModelError(string.Empty, "Token de autenticação ausente.");
                return Page();
            }

            HttpClientHandler handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;

            using (HttpClient client = new HttpClient(handler))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

                string apiUrl = "https://192.168.0.240:7256/api/usuario";

                HttpResponseMessage response = await client.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    ApiResponseContent = await response.Content.ReadAsStringAsync();
                    Usuarios = JsonConvert.DeserializeObject<List<UsuarioModel>>(ApiResponseContent);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Falha ao buscar usuários.");
                    ApiResponseContent = await response.Content.ReadAsStringAsync();
                }
            }

            return Page();
        }
    }
}
