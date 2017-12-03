using System;
using System.Web;
using Microsoft.AspNet.SignalR;
using SignalRModelLibrary;

namespace SignalRChat
{
    public class ChatHub : Hub
    {
        public void Send(ChatModel model)
        {
            // Call the broadcastMessage method to update clients.
            Clients.All.broadcastMessage(model);
        }
    }
}