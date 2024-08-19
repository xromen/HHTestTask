using Api.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly ILogger _logger;

        public MessageController(ILogger<MessageController> logger)
        {
            _logger = logger;
        }
        // GET: api/<MessageController>
        [HttpGet]
        public async Task<IEnumerable<Message>> Get()
        {
            _logger.LogInformation("Get all messages");
            DataBase db = await DataBase.BuildDataBaseAsync();
            return await db.GetAllMessages();
        }

        // GET api/<MessageController>/5
        [HttpGet("{from}/{to}")]
        public async Task<IEnumerable<Message>> Get(DateTime from, DateTime to)
        {
            _logger.LogInformation($"Get messages from {from} to {to}");
            DataBase db = await DataBase.BuildDataBaseAsync();
            return await db.GetMessages(from, to);
        }

        // POST api/<MessageController>
        [HttpPost]
        public async Task Post([FromBody] Message message)
        {
            _logger.LogInformation("Send new message");
            message.CreatedAt = DateTime.UtcNow;

            DataBase db = await DataBase.BuildDataBaseAsync();

            await db.AddMessage(message);

            await WebSocketHandler.SendMessage(message.CreatedAt.ToString() + " - " + message.SerialNumber + " - " + message.Text);
        }

        // PUT api/<MessageController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/<MessageController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
