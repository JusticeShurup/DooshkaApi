namespace BLL.AuthLogic.DTOS
{
    public class UserDTO
    {
        public required Guid Id { get; set; }

        public required string Email { get; set; }

        public string? Name { get; set; }

        public string AccessToken { get; set; } = "";

        public string RefreshToken { get; set; } = "";

    }
}
