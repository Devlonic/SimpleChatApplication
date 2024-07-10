using Microsoft.AspNetCore.Mvc;
using SimpleChatApplication.Api.Dto;
using SimpleChatApplication.DAL.Data.UnitOfWorks;
using SimpleChatApplication.DAL.Entities;
using SimpleChatApplication.DAL.Interfaces;

namespace SimpleChatApplication.Api.Controllers {
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class UserController : ControllerBase {
        private readonly ILogger<UserController> logger;
        private readonly IUnitOfWorkFactory unitOfWorkFactory;

        public UserController(ILogger<UserController> logger, IUnitOfWorkFactory unitOfWorkFactory) {
            this.logger = logger;
            this.unitOfWorkFactory = unitOfWorkFactory;
        }

        [HttpPost]
        public async Task<ActionResult> SignIn(SignInRequestDto dto) {
            using var unitOfWork = unitOfWorkFactory.CreateUnitOfWork();
            var user = new UserEntity() {
                UserName = dto.UserName,
            };
            await unitOfWork.GetRepository<UserEntity>().InsertAsync(user);

            await unitOfWork.CommitAsync();
            return Ok(user.Id);
        }
    }
}
