namespace SimpleChatApplication.Api.Dto.Users {
    public class SignInRequestDto {
        public string UserName { get; set; } = string.Empty;
    }
    public class SignInResponseDto {
        public int UserId { get; set; }
    }
}
