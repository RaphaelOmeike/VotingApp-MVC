using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using VotingApp.Models.RequestModels;
using VotingApp.Models.ResponseModels;
using VotingApp.Services.Interfaces;

namespace VotingApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RuleController : Controller
    {
        private readonly IRuleService _ruleService;
        private readonly ICourseService _courseService;
        public RuleController(IRuleService ruleService, ICourseService courseService)
        {
            _ruleService = ruleService;
            _courseService = courseService;
        }
        public IActionResult Create()
        {
            var response = _courseService.GetAllCourses();
            if (response.Data == null)
            {
                TempData["error"] = response.Message;
                return RedirectToAction("Create", "Course");
            }
            var courses = response.Data.ToList();
            ViewData["Course"] = new SelectList(courses, "Id", "Name");
            return View();
        }
        public IActionResult Update(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var response = _ruleService.GetUpdateRule((Guid)id);
            if (response.Data == null)
            {
                TempData["error"] = response.Message;
                return NotFound();//
            }
            var response2 = _courseService.GetAllCourses();
            var courses = response2.Data?.ToList();
            ViewData["Course"] = new SelectList(courses, "Id", "Name");
            ViewBag.RuleId = id;
            return View(response.Data);
        }
        public IActionResult GetAll(int? pageNumber)
        {
            var response = _ruleService.GetAllRules();
            if (response.Data == null)
            {
                TempData["warning"] = "No active rules!";
                return View();
            }
            int pageSize = 25;
            return View(PaginatedList<RuleResponseModel>.Create(response.Data.ToList(), pageNumber ?? 1, pageSize));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(RuleRequestModel request)
        {
            if (request.MinLevel > request.MaxLevel)
            {
                TempData["error"] = "Minimum level must be less than maximum level!";
                return RedirectToAction("GetAll");
            }
            if (ModelState.IsValid)
            {
                request.Name = request.Name.ToLower();
                var response = _ruleService.CreateRule(request);
                if (response.Data == null)
                {
                    TempData["error"] = response.Message;
                    return View(request);
                }
                TempData["success"] = response.Message;
                return RedirectToAction("GetAll");
            }
            TempData["error"] = "Error! Could not create rule!";
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(Guid id, RuleRequestModel request)
        {
            if (request.MinLevel > request.MaxLevel)
            {
                TempData["error"] = "Minimum level must be less than maximum level!";
                return RedirectToAction("GetAll");
            }
            if (ModelState.IsValid)
            {
                request.Name = request.Name.ToLower();
                var response = _ruleService.UpdateRule(id, request);
                if (response.Data == null)
                {
                    TempData["error"] = response.Message;
                    return RedirectToAction("GetAll");
                }
                TempData["success"] = response.Message;
                return RedirectToAction("GetAll");
            }
            TempData["error"] = "Error! Could not update rule!";
            return RedirectToAction("GetAll");
        }
    }
}
