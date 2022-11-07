using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace techpro.common.hubs.configs {
   public static class HubUserConnected
    {
        public static List<GroupModel> ConnectedGroups = new List<GroupModel> { };
    }
    public class GroupModel
    {
        public string id_group { get; set; }
        public string id_user { get; set; }
    }
    public abstract class common_hub : Hub {
        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            var removeUser = HubUserConnected.ConnectedGroups.Where(d => d.id_user == Context.ConnectionId).FirstOrDefault();
            HubUserConnected.ConnectedGroups.Remove(removeUser);
            return base.OnDisconnectedAsync(exception);
        }
        public async Task<string> joinRoom(string id_group)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, id_group);
            HubUserConnected.ConnectedGroups.Add(new GroupModel { id_group = id_group, id_user = Context.ConnectionId });
            return id_group;
        }
        public async Task<string> outRoom(string id_group)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, id_group);
            var removeUser = HubUserConnected.ConnectedGroups.Where(d => d.id_group == id_group).Where(d => d.id_user == Context.ConnectionId).FirstOrDefault();
            HubUserConnected.ConnectedGroups.Remove(removeUser);
            return id_group;
        }
    }
}
