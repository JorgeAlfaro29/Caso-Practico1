using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PAW3CP1.Core.BusinessLogic;

namespace PAW3CP1.Mvc.Controllers
{
    // Desactivado para pruebas hasta que el manejo de roles funcione
    // [Authorize(Roles = "Manager,SystemAdmin")]
    public class ApprovalController : Controller
    {
        private readonly ITaskBusiness business;

        public ApprovalController(ITaskBusiness business)
        {
            this.business = business;
        }

        //  esta seria la Vista principal de aprobaciones
        public async Task<IActionResult> Index()
        {
            var tasks = await business.GetAllAsync();
            return View(tasks);
        }

        // en esta parte deberia de  Actualizar el estado de una tarea (Aprobar/Denegar) // sigo probandolo 
        [HttpPut("/approvals/{id}")]
        public async Task<IActionResult> UpdateStatus(int id, [FromQuery] string status)
        {
            // Simula el rol actual
            var role = User.IsInRole("Manager") ? "Manager" : "User";

            var result = await business.UpdateApprovalStatusAsync(id, status, role);

            if (result == "OK")
                return Ok(new { message = $"Estado actualizado a '{status}'" });

            return BadRequest(new { message = result });
        }
    }
}