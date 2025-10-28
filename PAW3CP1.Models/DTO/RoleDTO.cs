using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PAW3CP1.Models.DTO
{
    public class RoleDTO
    {
        [JsonPropertyName("roleId")]
        public int RoleId { get; set; }

        [JsonPropertyName("roleName")]
        public string RoleName { get; set; } = null!;

        [JsonPropertyName("description")]
        public string? Description { get; set; }
    }
}
