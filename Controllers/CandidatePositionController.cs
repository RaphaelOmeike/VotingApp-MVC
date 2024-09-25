using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using VotingApp.Models.ResponseModels;
using VotingApp.Services.Interfaces;

namespace VotingApp.Controllers
{
    public class CandidatePositionController : Controller
    {
        private readonly ICandidatePositionService _candidatePositionService;
        public CandidatePositionController(ICandidatePositionService candidatePositionService)
        {
            _candidatePositionService = candidatePositionService;
        }
        [Authorize(Roles = "Admin")]
        public IActionResult GetAll(int? pageNumber)
        {
            var response = _candidatePositionService.GetAllCandidatePositions();
            if (response.Data == null)
            {
                TempData["warning"] = "No active contestants!";
                return View();
            }
            int pageSize = 10;

            return View(PaginatedList<CandidatePositionResponseModel>.Create(response.Data.ToList(), pageNumber ?? 1, pageSize));
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ChangeContestStatus(Guid id)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var response = _candidatePositionService.ChangeContestantStatus(id, userId);
            if (response.Data == null)
            {
                TempData["error"] = response.Message;
                return RedirectToAction("GetAll");
            }
            TempData["success"] = response.Message;
            if (User.IsInRole("Admin"))
            {
                return RedirectToAction("GetAll");

            }
            return RedirectToAction("MyCampaigns", "Election");
        }
    }
}
