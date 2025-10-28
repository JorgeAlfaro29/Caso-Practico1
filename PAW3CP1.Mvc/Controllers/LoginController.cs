using Microsoft.AspNetCore.Mvc;
using PAW3CP1.Mvc.ServiceApi;

namespace PAW3CP1.Mvc.Controllers
{
    public class LoginController : Controller
    {
        private readonly IUserService _userService;

        public LoginController(IUserService userService)
        {
            _userService = userService;
        }


        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> TryLogin(string email, string password)
        {
            try
            {
                var user = await _userService.CheckLogin(email);

                // Validar contraseña (si aplica)
                if (user == null || password != "12345") //password simulado
                {
                    ViewBag.Error = "Correo o contraseña incorrectos.";
                    return View("Login");
                }

                // TempData
                TempData["UserId"] = user.UserId;  
                TempData["Username"] = user.Username;
                TempData["UserEmail"] = user.Email;
                TempData["UserFullName"] = user.FullName;
                TempData["UserIsActive"] = user.IsActive;
                TempData["UserCreatedAt"] = user.CreatedAt.ToString("g");
                TempData["UserLastLogin"] = user.LastLogin?.ToString("g") ?? "Nunca";

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Correo o contraseña incorrectos.";
                return View("Login");
            }
        }
    }
}
