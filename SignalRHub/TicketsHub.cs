using Microsoft.AspNetCore.SignalR;

namespace SagErpBlazor.SignalRHub
{
    public class TicketsHub : Hub
    {
        public async Task NotifyDataChangedOnParent()
        {
            
            await Clients.All.SendAsync("DataChanged");
        }
    }
}
