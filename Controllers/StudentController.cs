using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using VotingApp.Models.RequestModels;
using VotingApp.Models.ResponseModels;
using VotingApp.Services.Interfaces;

namespace VotingApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class StudentController : Controller
    {
        private readonly IStudentService _studentService;
        private readonly ICourseService _courseService;

        public StudentController(IStudentService studentService, ICourseService courseService)
        {
            _studentService = studentService;
            _courseService = courseService;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ReadStudents(IFormFile studentFile)
        {
            if (studentFile != null)
            {
                var response = _studentService.ReadStudentsFromFile(studentFile);
                if (response.Status)
                {
                    TempData["success"] = response.Message;
                    return RedirectToAction("GetAll");
                }
                TempData["error"] = response.Message;
                return RedirectToAction("GetAll");
            }
            TempData["error"] = "Problem reading file!";
            return RedirectToAction("GetAll");
        }
        public IActionResult GetAll(int? pageNumber)
        {
            var response = _studentService.GetAllStudents();
            if (response.Data == null)
            {
                TempData["error"] = response.Message;
                return NotFound();
            }
            int pageSize = 20;

            return View(PaginatedList<StudentResponseModel>.Create(response.Data.ToList(), pageNumber ?? 1, pageSize));
        }
        public IActionResult Create()
        {
            var response = _courseService.GetAllCourses();
            if (response.Data == null)
            {
                TempData["error"] = response.Message;
                return RedirectToAction("Create", "Course");
            }
            var courses = response.Data.Where(c => c.Name != "all courses").ToList();
            ViewData["Course"] = new SelectList(courses, "Id", "Name");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(StudentRequestModel request)
        {
            if (ModelState.IsValid)
            {
                request.Email = request.Email.ToLower();
                request.MatricNo = request.MatricNo.ToLower();
                var response = _studentService.CreateStudent(request);
                if (response.Data == null)
                {
                    TempData["error"] = response.Message;
                    return View(request);
                }
                TempData["success"] = response.Message;
                return RedirectToAction("GetAll");
            }
            TempData["error"] = "Error! Could not create student!";
            return View();
        }
        public IActionResult Update(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var response = _studentService.GetUpdateStudent((Guid)id);
            if (response.Data == null)
            {
                TempData["error"] = response.Message;
                return NotFound();//
            }
            var response2 = _courseService.GetAllCourses();
            var courses = response2.Data?.Where(c => c.Name != "all courses").ToList();
            ViewData["Course"] = new SelectList(courses, "Id", "Name");
            ViewBag.StudentId = id;
            return View(response.Data);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(Guid id, UpdateStudentRequestModel request)
        {
            if (ModelState.IsValid)
            {
                var response = _studentService.UpdateStudent(id, request);
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ChangeVoteStatus(Guid id)
        {
            var response = _studentService.ChangeVoteStatus(id);
            if (response.Data == null)
            {
                TempData["error"] = response.Message;
                return RedirectToAction("GetAll");
            }
            TempData["success"] = response.Message;
            return RedirectToAction("GetAll");
        }
    }
}
