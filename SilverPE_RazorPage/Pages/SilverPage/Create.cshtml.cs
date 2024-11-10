using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using NuGet.Common;
using SilverPE_BOs.Models;
using SilverPE_RazorPage.Pages.Login;
using SilverPE_Repository.Request;
using SilverPE_Repository.Response;

namespace SilverPE_RazorPage.Pages.SilverPage
{
    public class CreateModel : PageModel
    {
        private readonly HttpClient _httpClient;
        public List<Category> Category { get; set; } = new List<Category>();

        public CreateModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async  Task<IActionResult> OnGet()
        {
            await GetCategoriesAsync();
        ViewData["CategoryId"] = new SelectList(Category, "CategoryId", "CategoryName");
            return Page();
        }

        [BindProperty]
        public CreateSilverJewerlryRequest SilverJewelry { get; set; } = default!;

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var token = HttpContext.Session.GetString("token");
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToPage("/logout/index");
            }

            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var json = JsonSerializer.Serialize(SilverJewelry);
            Console.WriteLine(json);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{Const.apiUrl}/api/SilverJewelry", content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("./Index");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Validation Error");
                await OnGet();
                return Page();
            }
        }

        private async Task<IActionResult> GetCategoriesAsync()
        {
            var token = HttpContext.Session.GetString("token");
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToPage("/logout/index");
            }

            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync($"{Const.apiUrl}/api/Category");

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return RedirectToPage("/logout/index");
            }
            else if (response.IsSuccessStatusCode)
            {
                var jsonData = await response.Content.ReadAsStringAsync();
                Category = JsonSerializer.Deserialize<List<Category>>(jsonData, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? new List<Category>();
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Error fetching data from API.");
            }
            return Page();
        }
    }
}
