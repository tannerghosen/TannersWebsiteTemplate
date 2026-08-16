// https://github.com/dotnet/AspNetCore.Docs/blob/main/aspnetcore/fundamentals/websockets/samples/8.x/WebSocketsSample/Controllers/WebSocketController.cs
using Microsoft.AspNetCore.Mvc;
using System.Net.WebSockets;
using System.Text;
using System.Collections.Concurrent;

namespace TannersWebsiteTemplate.Controllers
{
    public class WebSocketController : ControllerBase
    {
        public static ConcurrentDictionary<Guid, WebSocket> WebSocketConnections = new ConcurrentDictionary<Guid, WebSocket>();
        [Route("/ws")]
        public async Task Get()
        {
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                using var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();

                var id = Guid.NewGuid();
                // Add this new WebSocket connection to the list
                WebSocketConnections.TryAdd(id, webSocket);

                try
                {
                    await webSocket.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes((Status.GetStatus() == "" ? "" : Status.GetStatus()))), WebSocketMessageType.Text, true, CancellationToken.None);
                }
                catch (WebSocketException wse)
                {
                    // If an error occurs, remove it
                    WebSocketConnections.TryRemove(id, out _);
                    await Logger.Write(wse.Message, "WEBSOCKET");
                }
                await Echo(webSocket);
            }
            else
            {
                HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            }
        }

        private static async Task Echo(WebSocket webSocket)
        {
            try
            {
                var buffer = new byte[1024 * 4];
                var receiveResult = await webSocket.ReceiveAsync(
                    new ArraySegment<byte>(buffer), CancellationToken.None);

                while (!receiveResult.CloseStatus.HasValue)
                {
                    await webSocket.SendAsync(
                        new ArraySegment<byte>(buffer, 0, receiveResult.Count),
                        receiveResult.MessageType,
                        receiveResult.EndOfMessage,
                        CancellationToken.None);

                    var message = Encoding.UTF8.GetString(buffer, 0, receiveResult.Count);
                    if (message.Contains(Status.GetAccessPassword())) // did the client provide the access password?
                    {
                        // Good, that means they're allowed to change this.
                        int index = message.IndexOf(Status.GetAccessPassword()) - 1; // get the index of where AccessPassword starts - 1 (we include the space) in the message
                        message = message.Remove(index, Status.GetAccessPassword().Length + 1); // remove from the index to the end of the accesspassword length's + 1. (again, we include the space)
                        if (message == "clear")  // if the message is clear, reset it back to an empty string
                        {
                            Status.SetStatus("");
                        }
                        else // update status
                        {
                            Status.SetStatus(message);
                        }
                        // Iterate through each connection and send them any updated message
                        foreach (var connection in WebSocketConnections)
                        {
                            if (connection.Value.State == WebSocketState.Open)
                            {
                                _ = connection.Value.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(Status.GetStatus())), WebSocketMessageType.Text, true, CancellationToken.None);
                            }
                        }
                    }

                    receiveResult = await webSocket.ReceiveAsync(
                        new ArraySegment<byte>(buffer), CancellationToken.None);
                }
                    await webSocket.CloseAsync(
                        receiveResult.CloseStatus.Value,
                        receiveResult.CloseStatusDescription,
                        CancellationToken.None);
            }
            catch (WebSocketException wse)
            {
                await Logger.Write(wse.Message, "WEBSOCKET");
            }
        }
    }
}
