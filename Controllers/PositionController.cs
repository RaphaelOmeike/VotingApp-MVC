using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using VotingApp.Models.RequestModels;
using VotingApp.Models.ResponseModels;
using VotingApp.Services.Interfaces;

namespace VotingApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PositionController : Controller
    {
        private readonly IPositionService _positionService;
        private readonly IElectionService _electionService;
        private readonly IRuleService _ruleService;
        public PositionController(IPositionService positionService, IElectionService electionService, IRuleService ruleService)
        {
            _positionService = positionService;
            _electionService = electionService;
            _ruleService = ruleService;
        }
        public IActionResult GetAll(int? pageNumber)
        {
            var response = _positionService.GetAllPositions();
            if (response.Data == null)
            {
                TempData["warning"] = "No active positions!";
                return View();
            }
            int pageSize = 10;
            return View(PaginatedList<PositionResponseModel>.Create(response.Data.ToList(), pageNumber ?? 1, pageSize));
        }
        public IActionResult Create()
        {
            var response = _electionService.GetAllElections();
            if (response.Data == null)
            {
                TempData["error"] = response.Message;
                return RedirectToAction("Create", "Election");
            }
            var response2 = _ruleService.GetAllRules();
            if (response2.Data == null)
            {
                TempData["error"] = response2.Message;
                return RedirectToAction("Create", "Rule");
            }
            var elections = response.Data.Where(c => DateTime.Now < c.StartDate).ToList();
            var rules = response2.Data.ToList();
            ViewData["Election"] = new SelectList(elections, "Id", "Name");
            ViewData["Rule"] = new SelectList(rules, "Id", "Name");
            return View();
        }
        public IActionResult Update(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var response = _positionService.GetUpdatePosition((Guid)id);
            if (response.Data == null)
            {
                TempData["error"] = response.Message;
                return NotFound();//
            }
            var response2 = _electionService.GetAllElections();
            var response3 = _ruleService.GetAllRules();

            var elections = response2.Data?.Where(c => DateTime.Now < c.StartDate).ToList();
            var rules = response3.Data?.ToList();
            ViewData["Election"] = new SelectList(elections, "Id", "Name");
            ViewData["Rule"] = new SelectList(rules, "Id", "Name");
            ViewBag.PositionId = id;
            return View(response.Data);
        }
        //enable in view
        //public IActionResult ChangeAvailableStatus(Guid? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }
        //    var response = _positionService.ChangePositionStatus((Guid)id);
        //    if (response.Data == null)
        //    {
        //        TempData["error"] = response.Data;
        //        return NotFound();//
        //    }
        //    TempData["success"] = response.Data;
        //    return RedirectToAction("GetAll");
        //}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(PositionRequestModel request)
        {
            if (ModelState.IsValid)
            {
                request.Name = request.Name.ToLower();
                var response = _positionService.CreatePosition(request);
                if (response.Data == null)
                {
                    TempData["error"] = response.Message;
                    return RedirectToAction("GetAll");
                }
                TempData["success"] = response.Message;
                return RedirectToAction("GetAll");//
            }
            TempData["error"] = "Error! Could not create position!";
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(Guid id, PositionRequestModel request)
        {
            if (ModelState.IsValid)
            {
                request.Name = request.Name.ToLower();
                var response = _positionService.UpdatePosition(id, request);
                if (response.Data == null)
                {
                    TempData["error"] = response.Message;
                    return RedirectToAction("GetAll");
                }
                TempData["success"] = response.Message;
                return RedirectToAction("GetAll");
            }
            TempData["error"] = "Error! Could not update position!";
            return View(request);
        }

    }
}
