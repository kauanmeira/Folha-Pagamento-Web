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
    public class CargoModel : PageModel
    {
        [BindProperty]
        public int Id { get; set; }
        [BindProperty]
        public string Nome { get; set; }


        public List<CargoModel> Cargos { get; set; }
        public string ApiResponseContent { get; set; }

        [TempData]
        public string Token { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
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

                string apiUrl = "https://192.168.0.240:7256/api/cargos";

                HttpResponseMessage response = await client.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    ApiResponseContent = await response.Content.ReadAsStringAsync();
                    Cargos = JsonConvert.DeserializeObject<List<CargoModel>>(ApiResponseContent);


                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Falha ao buscar cargos.");
                    ApiResponseContent = await response.Content.ReadAsStringAsync();
                }
            }

            return Page();
        }

    }
}
