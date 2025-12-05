namespace RecipeCraftApi.Application.DTOs
{
    public class UserRegisterDto
    {
        public string? Name { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public bool IsPublic { get; set; } = true;
    }

    public class UserLoginDto
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
    }

    public class UserResponseDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Role { get; set; }
        public bool IsPublic { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class UserUpdateDto
    {
        public string? Name { get; set; }
        public string? LastName { get; set; }
        public string? Password { get; set; }
        public int? RoleId { get; set; }
        public bool? IsPublic { get; set; }
    }
}
