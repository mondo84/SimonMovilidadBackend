using Microsoft.AspNetCore.SignalR;

namespace IotApp.Hubs
{
    public class AlertsHub : Hub
    {
        public AlertsHub() { }

        public override async Task OnConnectedAsync()
        {
            Console.WriteLine($"Cliente conectado: {Context.ConnectionId}");
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            Console.WriteLine($"Cliente desconectado: {Context.ConnectionId}");
            if (exception != null)
            {
                Console.WriteLine($"Error: {exception.Message}");
            }

            await base.OnDisconnectedAsync(exception);
        }
    }
}
