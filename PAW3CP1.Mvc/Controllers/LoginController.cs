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

                // Session https://learn.microsoft.com/en-us/aspnet/core/fundamentals/app-state?view=aspnetcore-9.0
                HttpContext.Session.SetInt32("UserId", user.UserId);
                HttpContext.Session.SetString("Username", user.Username ?? "");
                HttpContext.Session.SetString("UserEmail", user.Email ?? "");
                HttpContext.Session.SetString("UserFullName", user.FullName ?? "");
                HttpContext.Session.SetString("UserIsActive", user.IsActive.ToString());
                HttpContext.Session.SetString("UserCreatedAt", user.CreatedAt.ToString("g"));
                HttpContext.Session.SetString("UserLastLogin", user.LastLogin?.ToString("g") ?? "Nunca");

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Correo o contraseña incorrectos.";
                return View("Login");
            }
        }

        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Login");
        }

    }
}
