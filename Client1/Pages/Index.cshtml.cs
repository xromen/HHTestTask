using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Client1.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        public bool IsWorking { get { return MessagesSender.IsWorking; } }

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            
        }

        public void OnPost()
        {
            if (MessagesSender.IsWorking)
            {
                _logger.LogInformation("MessagesSender ON");
                MessagesSender.StopSend();
            }
            else
            {
                _logger.LogInformation("MessagesSender OFF");
                MessagesSender.StartSend();
            }
        }
    }
}