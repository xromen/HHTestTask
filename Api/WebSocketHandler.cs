using System;
using System.Net.WebSockets;
using System.Text;

namespace Api
{
    public class WebSocketHandler
    {
        // Список всех клиентов
        private static readonly List<WebSocket> Clients = new List<WebSocket>();

        // Блокировка для обеспечения потокабезопасности
        private static readonly ReaderWriterLockSlim Locker = new ReaderWriterLockSlim();

        public async static Task AddClient(WebSocket client)
        {
            // Добавляем его в список клиентов
            Locker.EnterWriteLock();
            try
            {
                Clients.Add(client);
            }
            finally
            {
                Locker.ExitWriteLock();
            }

            var buffer = new byte[1024 * 4];
            WebSocketReceiveResult result = await client.ReceiveAsync(new ArraySegment<byte>(buffer),
                    CancellationToken.None);
        }

        private static void RemoveClient(WebSocket client)
        {
            // Добавляем его в список клиентов
            Locker.EnterWriteLock();
            try
            {
                Clients.Remove(client);
            }
            finally
            {
                Locker.ExitWriteLock();
            }
        }

        public async static Task SendMessage(string message)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(message);
            for (int i = 0; i < Clients.Count; i++)
            {
                WebSocket client = Clients[i];
                try
                {
                    await client.SendAsync(new ArraySegment<byte>(bytes, 0, message.Length),
                        WebSocketMessageType.Text,
                        true,
                        CancellationToken.None);
                }
                catch (Exception ex)
                {
                    RemoveClient(client);
                    i--;
                }
            }
        }

        public async static Task ChatHandler(WebSocket webSocket)
        {
            await AddClient(webSocket);

            try
            {
                var buffer = new byte[1024 * 4];
                WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer),
                    CancellationToken.None);
                while (!result.CloseStatus.HasValue)
                {
                    foreach (var client in Clients)
                    {
                        await client.SendAsync(new ArraySegment<byte>(buffer, 0, result.Count),
                            result.MessageType,
                            result.EndOfMessage,
                            CancellationToken.None);
                    }

                    result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer),
                        CancellationToken.None);
                }
                await webSocket.CloseAsync(result.CloseStatus.Value,
                    result.CloseStatusDescription,
                    CancellationToken.None);
            }
            finally
            {
                RemoveClient(webSocket);
            }
            
        }
    }
}
