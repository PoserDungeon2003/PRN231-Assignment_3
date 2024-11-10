using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SilverPE_Repository.Response;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace SilverPE_RazorPage.Pages.Login
{
    public class LoginModel
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }

    public class IndexModel : PageModel
    {
        private readonly HttpClient _httpClient;

        [BindProperty]
        public LoginModel LoginModel { get; set; } = new LoginModel();

        public IndexModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var loginData = new
            {
                Email = LoginModel.Email,
                Password = LoginModel.Password
            };

            var json = JsonSerializer.Serialize(loginData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{Const.apiUrl}/api/Account/login", content);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var loginResponse = JsonSerializer.Deserialize<AccountLoginResponse>(responseContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (loginResponse.Role != 1 && loginResponse.Role != 2)
                {
                    ModelState.AddModelError(string.Empty, "You are not allowed to access this function!");
                    return Page();
                }

                HttpContext.Session.SetString("token", loginResponse.Token);
                HttpContext.Session.SetString("id", loginResponse.Id.ToString());

                return RedirectToPage("/silverpage/index");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return Page();
            }
        }
    }
}
