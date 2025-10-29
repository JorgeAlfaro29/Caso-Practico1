using PAW3CP1.Models.DTO;
using System.Text.Json.Serialization;

namespace PAW3CP1.Data.Models
{
    public partial class UserRoleDTO
    {
    [JsonPropertyName("userId")]
    public int UserId { get; set; }

    [JsonPropertyName("roleId")]
    public int RoleId { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("role")]
    public virtual RoleDTO Role { get; set; } = null!;

    [JsonIgnore]
    public virtual UserDTO User { get; set; } = null!;
    }
}
