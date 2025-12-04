using System;

namespace RecipeCraftApi.Domain.Entities
{
    public class User
    {
        public int Id { get; set; } // Auto-increment
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? PasswordHash { get; set; }
        public int RoleId { get; set; }
        public Role? Role { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
