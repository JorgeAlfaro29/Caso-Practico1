using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PAW3CP1.Data.Models;
using PAW3CP1.Models.DTO;
using PAW3CP1.Mvc.ServiceApi;

namespace PAW3CP1.Mvc.Controllers
{
    public class UserRoleController : Controller
    {
        private readonly IUserRoleService _userRoleService;
      

        public UserRoleController(IUserRoleService userRoleService)
        {
            _userRoleService = userRoleService;
        }

        public async Task<IActionResult> Index()
        {
            var model = await _userRoleService.GetUserRolesViewAsync();
            return View(model);
        }


        // POST: /UserRole/AssignRole
        [HttpPost]
        public async Task<IActionResult> SaveRole(int userId, int roleId)
        {
            try
            {
                var result = await _userRoleService.AssignRoleAsync(userId, roleId);
                if (!result)
                    ViewBag.Error = "No se pudo asignar el rol. Verifica las reglas.";
            }
            catch (Exception ex)
            {
                ViewBag.Error = $"Error al asignar rol: {ex.Message}";
            }

            return RedirectToAction("Index");
        }
    }
}

