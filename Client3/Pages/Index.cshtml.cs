using Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;

namespace Client3.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        public IEnumerable<Message>? Messages { get; set; }


        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public async Task OnGet()
        {
            DateTime to = DateTime.UtcNow; 
            DateTime from = to.AddMinutes(-10);

            HttpClient _client = new HttpClient();
            _client.DefaultRequestHeaders.Add("Accept", "application/json");

            using HttpResponseMessage response = await _client.GetAsync($"http://api/api/Message/{from:yyyy-MM-ddTHH:mm:ssZ}/{to:yyyy-MM-ddTHH:mm:ssZ}");

            //string res = await response.Content.ReadAsStringAsync();

            Messages = await response.Content.ReadFromJsonAsync<IEnumerable<Message>>();
        }
    }
}