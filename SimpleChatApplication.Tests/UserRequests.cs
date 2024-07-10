using Microsoft.Extensions.DependencyInjection;
using Xunit;
using MediatR;
using SimpleChatApplication.BLL.CQRS.Users.Commands;
using FluentValidation;
namespace SimpleChatApplication.Tests {
    public class UserRequests : TestBase {
        [Fact]
        public async Task ShouldReturnTwoSamePrimaryKeys_WhenUsernameIsSame() {
            // Arranging

            var command = new SignInUserCommand() {
                UserName = "ShuttleX_BestCompany"
            };

            // Act
            var responce1 = await Mediator.Send(command);
            var responce2 = await Mediator.Send(command);

            // Assert
            Assert.True(responce1 == responce2, "Primary keys is not same");
        }

        [Fact]
        public async Task ShouldThrowValidationException_WhenUsernameExceedsSizeLimit() {
            // Arranging

            var command = new SignInUserCommand() {
                UserName = "ShuttleX_BestCompanyAndIWouldLikeToBeInYourTeam"
            };

            // Act
            // Assert
            await Assert.ThrowsAsync<ValidationException>(async () => {
                await Mediator.Send(command);
            });
        }
    }
}
