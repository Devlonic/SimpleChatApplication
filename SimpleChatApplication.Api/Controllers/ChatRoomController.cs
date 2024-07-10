using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimpleChatApplication.Api.Dto.ChatRooms;
using SimpleChatApplication.Api.Dto.Users;
using SimpleChatApplication.Api.Filters;
using SimpleChatApplication.BLL.CQRS.ChatRooms.Commands;
using SimpleChatApplication.BLL.CQRS.Users.Commands;

namespace SimpleChatApplication.Api.Controllers {
    [Route("api/[controller]/[action]")]
    [ApiController]
    [ApiExceptionFilter]
    public class ChatRoomController : ControllerBase {
        private readonly ILogger<ChatRoomController> logger;
        private readonly IMediator mediator;

        public ChatRoomController(ILogger<ChatRoomController> logger, IMediator mediator) {
            this.logger = logger;
            this.mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult> CreateRoom(CreateChatRoomRequestDto dto) {
            var chatRoomId = await mediator.Send(new CreateChatRoomUserCommand() {
                Title = dto.Title,
                CreatorId = dto.UserId,
            });

            return Ok(new CreateChatRoomResponceDto() {
                CreatedChatId = chatRoomId
            });
        }


    }
}
