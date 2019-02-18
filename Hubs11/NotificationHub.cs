using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

using Microsoft.AspNet.SignalR.Hubs;

namespace Requerimientos.Hubs
{
    public class NotificationHub : Hub
    {
        public static void Send()
        {

            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
            context.Clients.All.displayStatus();
        }
    }
}