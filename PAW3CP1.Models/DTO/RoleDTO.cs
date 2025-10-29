using System.Text.Json.Serialization;


namespace PAW3CP1.Data.Models;

public partial class RoleDTO
{
    [JsonPropertyName("roleId")]
    public int RoleId { get; set; }

    [JsonPropertyName("roleName")]
    public string RoleName { get; set; } = null!;

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("userRoles")]
    public virtual ICollection<UserRoleDTO> UserRoles { get; set; } = new List<UserRoleDTO>();
}
