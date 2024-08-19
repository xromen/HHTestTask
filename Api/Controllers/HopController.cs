using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Net.Sockets;
using System.Net.WebSockets;

namespace Api.Controllers
{
    [Route("ws/[controller]/[action]")]
    public class HopController : Controller
    {
        public HopController()
        {

        }

        /// <summary>
        /// Нужно для коннекта клиента
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task Connect()
        {
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                WebSocket webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();

                await WebSocketHandler.AddClient(webSocket);

                //await WebSocketHandler.ChatHandler(webSocket);
            }
            else
            {
                HttpContext.Response.StatusCode = 400;
            }
        }
    }
}
