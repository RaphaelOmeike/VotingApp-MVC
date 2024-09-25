using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VotingApp.Models.ResponseModels;
using VotingApp.Services.Interfaces;

namespace VotingApp.Controllers
{
    public class CandidateController : Controller
    {
        private readonly ICandidateService _candidateService;
        public CandidateController(ICandidateService candidateService)
        {
            _candidateService = candidateService;
        }
        [Authorize(Roles = "Admin")]
        public IActionResult GetAll(int? pageNumber)
        {
            var response = _candidateService.GetAllCandidates();
            if (response.Data == null)
            {
                TempData["warning"] = "No active candidates!";
                return View();
            }
            int pageSize = 3;
            return View(PaginatedList<CandidateResponseModel>.Create(response.Data.ToList(), pageNumber ?? 1, pageSize));
        }
    }
}
