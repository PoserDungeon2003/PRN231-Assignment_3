using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SilverPE_BOs.Models;
using SilverPE_Repository.Request;
using System.Text;

namespace SilverPE_RazorPage.Pages.SilverPage
{
    public class EditModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public EditModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [BindProperty]
        public UpdateSilverJewerlyRequest SilverJewelry { get; set; } = default!;
        public List<Category> Category { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            await GetCategoriesAsync();

            var token = HttpContext.Session.GetString("token");
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToPage("/logout/index");
            }

            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync($"{Const.apiUrl}/api/SilverJewelry/{id}");

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return RedirectToPage("/logout/index");
            }
            else if (response.IsSuccessStatusCode)
            {
                var jsonData = await response.Content.ReadAsStringAsync();
                SilverJewelry = JsonSerializer.Deserialize<UpdateSilverJewerlyRequest>(jsonData, new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve,
                    PropertyNameCaseInsensitive = true
                }) ?? new UpdateSilverJewerlyRequest();
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Error fetching data from API.");
            }

            ViewData["CategoryId"] = new SelectList(Category, "CategoryId", "CategoryName");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
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

            var response = await _httpClient.PutAsync($"{Const.apiUrl}/api/SilverJewelry/{id}", content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("./Index");
            }
            else
            {
                ModelState.AddModelError(string.Empty, $"Error: {response.StatusCode}");
                await OnGetAsync(id);
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
