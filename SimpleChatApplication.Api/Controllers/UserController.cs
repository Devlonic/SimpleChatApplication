using MediatR;
using Microsoft.AspNetCore.Mvc;
using SimpleChatApplication.Api.Dto;
using SimpleChatApplication.Api.Filters;
using SimpleChatApplication.BLL.CQRS.Users.Commands;
using SimpleChatApplication.DAL.Data.UnitOfWorks;
using SimpleChatApplication.DAL.Entities;
using SimpleChatApplication.DAL.Interfaces;

namespace SimpleChatApplication.Api.Controllers {
    [ApiController]
    [Route("api/[controller]/[action]")]
    [ApiExceptionFilter]
    public class UserController : ControllerBase {
        private readonly ILogger<UserController> logger;
        private readonly IMediator mediator;

        public UserController(ILogger<UserController> logger, IMediator mediator) {
            this.logger = logger;
            this.mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult> SignIn(SignInRequestDto dto) {
            var userId = await mediator.Send(new SignInUserCommand() {
                UserName = dto.UserName,
            });
            var user1Id = await mediator.Send(new SignInUserCommand() {
                UserName = dto.UserName,
            });
            return Ok(userId + user1Id);
        }
    }
}
