using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SilverPE_BOs.Models;

namespace SilverPE_RazorPage.Pages.SilverPage
{
    public class DetailsModel : PageModel
    {
        private readonly HttpClient _httpClient;
        public DetailsModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public SilverJewelry SilverJewelry { get; set; } = default!;

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
    }
}
