using Microsoft.AspNetCore.Mvc;
using SellingManagementSystem.Models;
using SellingManagementSystem.Services;

namespace SellingManagementSystem.Controllers
{
    public class LoginController : Controller
    {
        private IUserService _userService;
        public LoginController(IUserService userService)
        {
            _userService = userService;
        }
        public IActionResult Index()
        {

            if (Request.Cookies["token"] != null) return RedirectToAction("Index", "Home");

            return View();
        }

        [HttpPost]
        public ActionResult Index(string email, string password)
        {
            User credentials = new User();

            credentials.Email = email;
            credentials.Password = password;

            string token = _userService.Auth(credentials);

            Response.Cookies.Append("token", token);

            return RedirectToAction("Index", "Home");
        }
    }
}
