using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SilverPE_BOs.Models;
using SilverPE_RazorPage;
using SilverPE_RazorPage.Pages.SilverPage;

namespace SilverPE_RazorPage.Pages.SilverPage
{
    public class DeleteResponse
    {
        public string? Message { get; set; }
        public bool Success { get; set; }
    }

    public class DeleteModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public DeleteModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [BindProperty]
        public SilverJewelry SilverJewelry { get; set; } = default!;
        public DeleteResponse DeleteResponse { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

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
                SilverJewelry = JsonSerializer.Deserialize<SilverJewelry>(jsonData, new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve,
                    PropertyNameCaseInsensitive = true
                }) ?? new SilverJewelry();
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Error fetching data from API.");
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var token = HttpContext.Session.GetString("token");
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToPage("/logout/index");
            }

            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.DeleteAsync($"{Const.apiUrl}/api/SilverJewelry/{id}");

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return RedirectToPage("/logout/index");
            }
            else if (response.IsSuccessStatusCode)
            {
                var jsonData = await response.Content.ReadAsStringAsync();
                DeleteResponse = JsonSerializer.Deserialize<DeleteResponse>(jsonData, new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve,
                    PropertyNameCaseInsensitive = true
                }) ?? new DeleteResponse();
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Error fetching data from API.");
            }
            return RedirectToPage("./Index");
        }
    }
}
