using MediCareCMS.Models;
using MediCareCMS.Service;
using Microsoft.AspNetCore.Mvc;

namespace MediCareCMS.Controllers
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
        public IActionResult Login(User model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                var user = _userService.Authenticate(model.Username, model.Password);

                if (user != null)
                {
                    HttpContext.Session.SetInt32("UserId", user.UserId);
                    HttpContext.Session.SetString("Username", user.Username);
                    HttpContext.Session.SetString("Role", user.Role);

                    // 👇 Redirect based on role
                    if (user.Role.Equals("Doctor", StringComparison.OrdinalIgnoreCase))
                    {
                        return RedirectToAction("TodayAppointments", "Doctor", new { doctorId = user.UserId });
                    }
                    else if (user.Role.Equals("Receptionist", StringComparison.OrdinalIgnoreCase))
                    {
                        return RedirectToAction("Index", "Reception");
                    }
                    else if (user.Role.Equals("Admin", StringComparison.OrdinalIgnoreCase))
                    {
                        return RedirectToAction("Index", "Admin");
                    }
                }

                ViewBag.Error = "Invalid username or password.";
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }

            return View(model);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
