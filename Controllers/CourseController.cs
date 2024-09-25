using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VotingApp.Models.RequestModels;
using VotingApp.Models.ResponseModels;
using VotingApp.Services.Interfaces;

namespace VotingApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CourseController : Controller
    {
        private readonly ICourseService _courseService;
        public CourseController(ICourseService courseService)
        {
            _courseService = courseService;
        }
        public IActionResult Create()
        {
            return View();
        }
        public IActionResult GetAll(int? pageNumber)
        {
            var response = _courseService.GetAllCourses();
            if (response.Data == null)
            {
                TempData["warning"] = "No active courses!";
                return View();
            }
            int pageSize = 25;
            return View(PaginatedList<CourseResponseModel>.Create(response.Data.ToList(), pageNumber ?? 1, pageSize));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CourseRequestModel request)
        {
            if (ModelState.IsValid)
            {
                request.Name = request.Name.ToLower();
                var response = _courseService.CreateCourse(request);
                if (response.Data == null)
                {
                    TempData["error"] = response.Message;
                    return View();
                }
                TempData["success"] = response.Message;
                return RedirectToAction("GetAll");//
            }
            TempData["error"] = "Error! Could not create course!";
            return View();
        }
        public IActionResult Update(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var response = _courseService.GetUpdateCourse((Guid)id);
            if (response.Data == null)
            {
                TempData["error"] = response.Message;
                return NotFound();//
            }
            ViewBag.CourseId = id;
            return View(response.Data);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(Guid id, CourseRequestModel request)
        {
            if (ModelState.IsValid)
            {
                request.Name = request.Name.ToLower();
                var response = _courseService.UpdateCourse(id, request);
                if (response.Data == null)
                {
                    TempData["error"] = response.Message;
                    return RedirectToAction("GetAll");
                }
                TempData["success"] = response.Message;
                return RedirectToAction("GetAll");
            }
            TempData["error"] = "Error! Could not update course!";
            return View(request);
        }
    }
}
