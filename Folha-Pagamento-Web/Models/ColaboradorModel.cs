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
    public class ColaboradorModel : PageModel
    {
        [BindProperty]
        public int Id { get; set; }
        [BindProperty]
        public string Nome { get; set; }
        [BindProperty]
        public string Sobrenome { get; set; }

        [BindProperty]
        public string Email { get; set; }
        [BindProperty]
        public double SalarioBase { get; set; }
        [BindProperty]
        public DateTime DataNascimento { get; set; }
        [BindProperty]
        public DateTime DataAdmissao { get; set; }
        [BindProperty]
        public DateTime? DataDemissao { get; set; }
        [BindProperty]
        public int Dependentes { get; set; }
        [BindProperty]
        public int Filhos { get; set; }

        [BindProperty]
        public int CargoId { get; set; }
        [BindProperty]
        public int EmpresaId { get; set; }
        [BindProperty]
        public bool Ativo { get; set; }
        [BindProperty]
        public string CEP { get; set; }
        [BindProperty]
        public string Logradouro { get; set; }
        [BindProperty]
        public string Bairro { get; set; }
        [BindProperty]
        public string Numero { get; set; }
        [BindProperty]
        public string Cidade { get; set; }
        [BindProperty]
        public string Estado { get; set; }






        public List<ColaboradorModel> Colaboradores { get; set; }
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

                string apiUrl = "https://192.168.0.240:7256/api/colaboradores";

                HttpResponseMessage response = await client.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    ApiResponseContent = await response.Content.ReadAsStringAsync();
                    Colaboradores = JsonConvert.DeserializeObject<List<ColaboradorModel>>(ApiResponseContent);


                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Falha ao buscar colaboradores.");
                    ApiResponseContent = await response.Content.ReadAsStringAsync();
                }
            }

            return Page();
        }

    }
}
