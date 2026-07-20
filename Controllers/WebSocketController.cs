// https://github.com/dotnet/AspNetCore.Docs/blob/main/aspnetcore/fundamentals/websockets/samples/8.x/WebSocketsSample/Controllers/WebSocketController.cs
using Microsoft.AspNetCore.Mvc;
using System.Net.WebSockets;
using System.Text;

namespace TannersWebsiteTemplate.Controllers
{
    public class WebSocketController : ControllerBase
    {
        [Route("/ws")]
        public async Task Get()
        {
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                using var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
                await webSocket.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes((Status.status == "" ? "" : Status.status))), WebSocketMessageType.Text, true, CancellationToken.None);
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
                            Status.status = String.Empty;
                        }
                        else // update status
                        {
                            Status.status = message;
                        }
                        if (webSocket.State == WebSocketState.Open)
                        {
                            await webSocket.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes((Status.status == "" ? "" : Status.status))), WebSocketMessageType.Text, true, CancellationToken.None);  // update everyone listening's Status
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
                Logger.Write(wse.Message, "WEBSOCKET");
                Console.WriteLine(wse.Message);
            }
            catch (Exception e)
            {
                Logger.Write(e.ToString(), "WEBSOCKET");
                Console.WriteLine(e.ToString());
            }
        }
    }
}
