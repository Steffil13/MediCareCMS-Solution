using MediCareCMS.Models;
using MediCareCMS.Service;
using MediCareCMS.ViewModel;
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
        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                foreach (var err in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(err.ErrorMessage); // Log to console for debugging
                }
                return View(model);
            }


            try
            {
                var user = _userService.Authenticate(model.Username, model.Password);

                if (user != null)
                {
                    // 💾 Store session
                    HttpContext.Session.SetInt32("UserId", user.UserId);
                    HttpContext.Session.SetString("Username", user.Username);
                    HttpContext.Session.SetString("Role", user.Role);

                    // ✅ Clean up role string
                    string role = user.Role?.Trim().ToLower();

                    switch (role)
                    {
                        case "admin":
                            return RedirectToAction("Index", "Admin");

                        case "receptionist":
                            return RedirectToAction("Index", "Reception");

                        case "doctor":
                            return RedirectToAction("TodayAppointments", "Doctor", new { doctorId = user.UserId });

                        case "pharmacist":
                            return RedirectToAction("Index", "Pharmacist");

                        case "lab":
                            return RedirectToAction("Index", "Lab");

                        default:
                            ViewBag.Error = $"Unrecognized role: {user.Role}";
                            break;
                    }
                }
                else
                {
                    ViewBag.Error = "Invalid username or password.";
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = $"Login failed: {ex.Message}";
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
