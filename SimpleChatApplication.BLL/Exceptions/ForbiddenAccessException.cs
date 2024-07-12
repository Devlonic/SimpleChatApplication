namespace SimpleChatApplication.BLL.Exceptions {
    public class ForbiddenAccessException : Exception {
        public ForbiddenAccessException() : base() { }

        public ForbiddenAccessException(string? message) : base(message) {
        }

        public ForbiddenAccessException(string? message, Exception? innerException) : base(message, innerException) {
        }

        public ForbiddenAccessException(int? requesterId, int? requestedKey, string entityName) : this($"Access denied for user {requesterId} for {entityName} with key {requestedKey}") {
        }
    }
}