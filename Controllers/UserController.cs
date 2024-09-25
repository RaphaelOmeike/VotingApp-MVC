using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using VotingApp.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using VotingApp.Models.RequestModels;
using VotingApp.Models.ResponseModels;

namespace VotingApp.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [Authorize(Roles = "Admin")]
        public IActionResult GetAll(int? pageNumber)
        {
            var response = _userService.GetAllUsers();
            if (response.Data == null)
            {
                TempData["warning"] = "No active users!";
                return View();
            }
            int pageSize = 3;
            return View(PaginatedList<UserResponseModel>.Create(response.Data.ToList(), pageNumber ?? 1, pageSize));
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(UserRequestModel request)
        {
            var response = _userService.LoginUser(request);
            if (response.Status)
            {
                if (response.Data == null)
                {
                    TempData["error"] = response.Message;
                    return View(request);
                }
                CookieOptions cookie = new CookieOptions();
                cookie.Expires = DateTime.Now.AddHours(1);
                Response.Cookies.Append("Id", response.Data.Id.ToString(), cookie);
                var user = response.Data;
                var claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, user.RoleName)
                };
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);
                var properties = new AuthenticationProperties();

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, properties);
                TempData["success"] = response.Message;
                return RedirectToAction("Index", "Home");//to be changed later on
            }
            TempData["error"] = response.Message;
            return View(request);
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            TempData["success"] = "Logged out successfully!";
            return RedirectToAction("Index", "Home");//to be changed later on
        }
        [Authorize(Roles = "Student")]
        public IActionResult UpdatePassword()
        {
            if (User.Identity.IsAuthenticated)
            {
                string? id = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (id != null)
                {
                    var userId = Guid.Parse(id);
                    ViewBag.UserId = userId;
                }
            }
            return View();
        }
        [Authorize(Roles = "Student")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdatePassword(Guid id, UpdateUserRequestModel request)
        {
            if (request.Password != request.CPassword)
            {
                TempData["error"] = "The password must be the same";
                return RedirectToAction("GetAll", "Election");
            }
            if (ModelState.IsValid)
            {
                var response = _userService.UpdatePassword(id, request);
                if (response.Data == null)
                {
                    TempData["error"] = response.Message;
                    return RedirectToAction("GetAll", "Election");
                }
                TempData["success"] = response.Message;
                return RedirectToAction("GetAll", "Election");
            }
            TempData["error"] = "An error occurred";
            return RedirectToAction("GetAll", "Election");
        }
    }
}
