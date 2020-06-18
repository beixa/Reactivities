using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Application.Comments;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace API.SignalR
{
    [Authorize]
    public class ChatHub : Hub
    {
        private readonly IMediator _mediator;
        public ChatHub(IMediator mediator)
        {
            _mediator = mediator;

        }

        public async Task SendComment(Create.Command command)
        {
            string username = Context.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;//GetUsername();

            command.Username = username;

            var comment = await _mediator.Send(command); //the client invokes SendComment and sends the command to mediatr, after is saved we get the comment and then this comment is sent to all the clients connected to the hub

            await Clients.All.SendAsync("ReceiveComment", comment);
            
            await Clients.Group(command.ActivityId.ToString()).SendAsync("ReceiveComment", comment);
        }

        private string GetUsername()
        {
            return Context.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
        }

        public async Task AddToGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            
            var username = GetUsername();

            await Clients.Group(groupName).SendAsync("Send",$"{username} has joined the group");
        }

        public async Task RemoveFromGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
            
            var username = GetUsername();

            await Clients.Group(groupName).SendAsync("Send",$"{username} has left the group");
        }
    }
}