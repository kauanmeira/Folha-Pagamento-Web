using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Folha_Pagamento_Web.Models
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public string Email { get; set; }

        [BindProperty]
        public string Senha { get; set; }

        public string ApiResponseContent { get; set; }

        // Propriedade Token para armazenar o token
        public string Token { get; set; }

        public async Task<IActionResult> OnPost()
        {
            // Configurar um manipulador HTTP para ignorar a validação do certificado SSL
            HttpClientHandler handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;

            using (HttpClient client = new HttpClient(handler))
            {
                // URL da API de autenticação
                string apiUrl = "https://192.168.0.240:7256/api/login";

                // Dados a serem enviados no corpo da solicitação
                var data = new { Email, Senha };
                var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");

                // Enviar a solicitação POST para a API
                HttpResponseMessage response = await client.PostAsync(apiUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    // A resposta da API foi bem-sucedida, então leia o token da resposta
                    ApiResponseContent = await response.Content.ReadAsStringAsync();
                    var jsonResponse = JsonConvert.DeserializeAnonymousType(ApiResponseContent, new { token = "" });

                    if (!string.IsNullOrEmpty(jsonResponse.token))
                    {
                        // Armazena o token em TempData para compartilhá-lo com outras páginas
                        TempData["Token"] = jsonResponse.token;

                        // Redireciona para a view Index
                        return RedirectToPage("/Index");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Token de autenticação ausente na resposta da API.");
                        ApiResponseContent = ""; // Limpar o conteúdo da resposta
                    }
                }
                else
                {
                    // A API retornou um status de erro (por exemplo, credenciais inválidas)
                    ModelState.AddModelError(string.Empty, "Credenciais inválidas.");
                    ApiResponseContent = await response.Content.ReadAsStringAsync();
                }
            }

            return Page();
        }
    }
}
