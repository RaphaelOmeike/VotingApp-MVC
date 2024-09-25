using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VotingApp.Models.RequestModels;
using VotingApp.Models.ResponseModels;
using VotingApp.Services.Interfaces;

namespace VotingApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SessionController : Controller
    {
        private readonly ISessionService _sessionService;
        public SessionController(ISessionService sessionService)
        {
            _sessionService = sessionService;
        }
        public IActionResult GetAll(int? pageNumber)
        {
            var response = _sessionService.GetAllSessions();
            if (response.Data == null)
            {
                TempData["warning"] = "No active sessions!";
                return View();
            }
            int pageSize = 3;
            return View(PaginatedList<SessionResponseModel>.Create(response.Data.ToList(), pageNumber ?? 1, pageSize));
        }
        public IActionResult Create()
        {
            return View();
        }
        public IActionResult Update(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var response = _sessionService.GetUpdateSession((Guid)id);
            if (response.Data == null)
            {
                TempData["error"] = response.Message;
                return NotFound();//
            }
            ViewBag.SessionId = id;
            return View(response.Data);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(SessionRequestModel request)
        {
            if (ModelState.IsValid)
            {
                request.Name = request.Name.ToLower();
                var response = _sessionService.CreateSession(request);
                if (response.Data == null)
                {
                    TempData["error"] = response.Message;
                    return View();
                }
                TempData["success"] = response.Message;
                return RedirectToAction("GetAll");//
            }
            TempData["error"] = "Error! Could not create session!";
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(Guid id, SessionRequestModel request)
        {
            if (ModelState.IsValid)
            {
                request.Name = request.Name.ToLower();
                var response = _sessionService.UpdateSession(id, request);
                if (response.Data == null)
                {
                    TempData["error"] = response.Message;
                    return RedirectToAction("GetAll");
                }
                TempData["success"] = response.Message;
                return RedirectToAction("GetAll");
            }
            TempData["error"] = "Error! Could not update session!";
            return View(request);
        }

    }
}
