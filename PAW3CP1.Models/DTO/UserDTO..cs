using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using PAW3CP1.Data.Models;
using PAW3CP1.Models.DTO;

namespace PAW3CP1.Models.DTO
{
    public class UserDTO
    {
        [JsonPropertyName("userId")]
        public int UserId { get; set; }

        [JsonPropertyName("username")]
        public string Username { get; set; } = null!;

        [JsonPropertyName("email")]
        public string Email { get; set; } = null!;

        [JsonPropertyName("fullName")]
        public string? FullName { get; set; }

        [JsonPropertyName("isActive")]
        public bool IsActive { get; set; }

        [JsonPropertyName("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("lastLogin")]
        public DateTime? LastLogin { get; set; }

        [JsonPropertyName("userRoles")]
        public virtual ICollection<UserRoleDTO> UserRoles { get; set; } = new List<UserRoleDTO>();
    }
}
