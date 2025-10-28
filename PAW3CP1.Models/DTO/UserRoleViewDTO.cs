using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PAW3CP1.Models.DTO
{
    public class UserRoleViewDTO
    {
        public int UserId { get; set; }
        public string Email { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public int CurrentRoleId { get; set; }
        public string CurrentRoleName { get; set; } = null!;
        public List<RoleDTO> AvailableRoles { get; set; } = new();
    }
}
