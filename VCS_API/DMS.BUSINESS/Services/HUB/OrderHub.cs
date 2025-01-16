using DMS.CORE.Entities.BU;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DMS.BUSINESS.Services.HUB
{
    public class OrderHub : Hub
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly List<Claim> _Claims;
        public OrderHub(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
            try
            {
                var token = _contextAccessor?.HttpContext?.Request?.Query["access_token"];
                if (!string.IsNullOrWhiteSpace(token))
                {
                    JwtSecurityTokenHandler tokenHandler = new();
                    JwtSecurityToken securityToken = (JwtSecurityToken)tokenHandler.ReadToken(token);
                    _Claims = securityToken.Claims.ToList();
                }
            }
            catch (Exception)
            {
                _Claims = [];
            }
        }

        private string UserName
        {
            get
            {
                try
                {
                    return _Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }
        public async Task ORDER_LIST_CHANGED(object data)
        {
            await Clients.All.SendAsync("ORDER_LIST_CHANGED", data);
        }
        public override async Task OnConnectedAsync()
        {
            if (!string.IsNullOrEmpty(UserName))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, UserName);
            }
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            if (!string.IsNullOrEmpty(UserName))
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, UserName);
            }
            await base.OnDisconnectedAsync(exception);
        }

        public async Task JoinGroup(string groupName)
        {
            try
            {
                if (!string.IsNullOrEmpty(groupName))
                {
                    await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
                    await Clients.Caller.SendAsync("JoinGroupSuccess", groupName);
                }
            }
            catch (Exception ex)
            {
                await Clients.Caller.SendAsync("JoinGroupError", ex.Message);
            }
        }

        public async Task LeaveGroup(string groupName)
        {
            try
            {
                if (!string.IsNullOrEmpty(groupName))
                {
                    await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
                    await Clients.Caller.SendAsync("LeaveGroupSuccess", groupName);
                }
            }
            catch (Exception ex)
            {
                await Clients.Caller.SendAsync("LeaveGroupError", ex.Message);
            }
        }
    }
}
