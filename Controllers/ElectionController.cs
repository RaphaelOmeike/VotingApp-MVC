using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using VotingApp.Models.RequestModels;
using VotingApp.Models.ResponseModels;
using VotingApp.Services.Interfaces;

namespace VotingApp.Controllers
{
    public class ElectionController : Controller
    {
        private readonly IElectionService _electionService;
        private readonly ISessionService _sessionService;
        private readonly IRuleService _ruleService;
        private readonly IPositionService _positionService;
        private readonly ICandidatePositionService _candidatePositionService;
        private readonly IStudentService _studentService;
        private readonly ICandidateService _candidateService;
        private readonly IVoteCastingInfoService _voteService;
        public ElectionController(IElectionService electionService, ISessionService sessionService, IRuleService ruleService, IPositionService positionService, ICandidatePositionService candidatePositionService, IStudentService studentService, ICandidateService candidateService, IVoteCastingInfoService voteService)
        {
            _electionService = electionService;
            _sessionService = sessionService;
            _ruleService = ruleService;
            _positionService = positionService;
            _candidatePositionService = candidatePositionService;
            _studentService = studentService;
            _candidateService = candidateService;
            _voteService = voteService;
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            var response = _sessionService.GetAllSessions();
            if (response.Data == null)
            {
                TempData["error"] = response.Message;
                return RedirectToAction("Create", "Session");
            }
            var response2 = _ruleService.GetAllRules();
            if (response2.Data == null)
            {
                TempData["error"] = response2.Message;
                return RedirectToAction("Create", "Rule");
            }
            var sessions = response.Data.ToList();
            var rules = response2.Data.ToList();
            ViewData["Session"] = new SelectList(sessions, "Id", "Name");
            ViewData["Rule"] = new SelectList(rules, "Id", "Name");
            return View();///
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Name,Description,Image,StartDate,EndDate,SessionId,RuleId")] ElectionRequestModel request)
        {
            if (request.StartDate >= request.EndDate || DateTime.Now >= request.StartDate || DateTime.Now >= request.EndDate)
            {
                TempData["error"] = "Error! Invalid date!";
                return RedirectToAction("GetAll");
            }
            if (request.Image != null)
            {
                request.Name = request.Name.ToLower();
                var response = _electionService.CreateElection(request);
                if (response.Data == null)
                {
                    TempData["error"] = response.Message;
                    return RedirectToAction("GetAll");
                }
                TempData["success"] = response.Message;
                return RedirectToAction("GetAll");
            }

            TempData["error"] = "Error! Could not create election!";
            return RedirectToAction("GetAll");
        }
        public IActionResult GetAll(int? pageNumber, string searchString)
        {
            var response = _electionService.GetAllElections();
            if (response.Data == null)
            {
                TempData["warning"] = "No active elections!";
                return View();
            }
            int pageSize = 10;
            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.Trim().ToLower();
                response.Data = response.Data.Where(c => c.Name.Contains(searchString)).ToList();
            }
            ViewBag.PageNo = pageNumber ?? 1;
            return View(PaginatedList<ElectionResponseModel>.Create(response.Data.ToList(), pageNumber ?? 1, pageSize));
        }
        public IActionResult Details(Guid electionId, int? pageNumber, int? electionPageNo)
        {
            var response = _positionService.GetAllPositionsForElection(electionId);
            if (response.Data == null)
            {
                TempData["error"] = response.Message;
                return NotFound();
            }
            int pageSize = 7;
            ViewBag.ElectionId = electionId;
            ViewBag.PageNo = electionPageNo ?? 1;
            //ViewBag.Contest = _positionService.StudentIsEligible()
            return View(PaginatedList<PositionResponseModel>.Create(response.Data.ToList(), pageNumber ?? 1, pageSize));
        }
        public IActionResult GetContestants(Guid positionId, int? pageNumber)
        {
            var response = _candidatePositionService.GetAllContestantsForPosition(positionId);
            if (response.Data == null)
            {
                TempData["error"] = response.Message;
                return NotFound();
            }
            int pageSize = 3;
            ViewBag.PositionId = positionId;
            var position = _positionService.GetPosition(positionId);
            ViewBag.ElectionId = position.Data?.ElectionId;
            if (User.Identity.IsAuthenticated)
            {
                string? email = User.FindFirstValue(ClaimTypes.Email);
                if (email != null)
                {
                    var stuExists = _studentService.GetStudentByEmail(email);
                    if (stuExists.Data != null)
                    {
                        var studentId = stuExists.Data.Id;
                        ViewBag.CanContest = _positionService.StudentIsEligible(studentId, positionId).Status;
                        ViewBag.StudentId = studentId;
                        
                    }
                }
            }
            return View(PaginatedList<CandidatePositionResponseModel>.Create(response.Data.ToList(), pageNumber ?? 1, pageSize));
        }
        [Authorize(Roles = "Student")]
        public IActionResult Contest(Guid studentId, Guid positionId)
        {
            var stuExists = _studentService.GetStudent(studentId);
            if (stuExists.Data != null)
            {
                ViewBag.StudentId = studentId;
                ViewBag.PositionId = positionId;
            }
            return View();
        }
        [Authorize(Roles = "Student")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Contest(CandidatePositionRequestModel request)
        {
            var response = _candidatePositionService.CreateCandidatePosition(request);
            if (response.Data == null)
            {
                TempData["error"] = response.Message;
                return RedirectToAction("GetAll");
            }
            TempData["success"] = response.Message;
            return RedirectToAction("GetContestants", new { positionId = request.PositionId });
        }
        [Authorize(Roles = "Student")]
        public IActionResult UpdateContest(Guid? id)//test today unfailingly by God's grace and Mother Mary's intercession and going back after voting
        {
            if (id == null)
            {
                return NotFound();
            }
            var response2 = _candidatePositionService.GetCandidatePositionById((Guid)id);
            if (response2.Data == null)
            {
                TempData["error"] = response2.Message;
                return RedirectToAction("MyCampaigns");
            }
            var response = _candidatePositionService.GetUpdateCandidatePosition((Guid)id);
            if (response.Data == null)
            {
                TempData["error"] = response.Message;
                return RedirectToAction("MyCampaigns");
            }
            if (DateTime.Now >= response2.Data.Position?.Election?.StartDate)
            {
                TempData["error"] = "Election already started!";
                return RedirectToAction("MyCampaigns");
            }
            return View(response.Data);
        }
        [Authorize(Roles = "Student")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateContest(CandidatePositionRequestModel request)
        {
            if (ModelState.IsValid)
            {
                var response = _candidatePositionService.UpdateCandidatePosition(request);
                if (response.Data == null)
                {
                    TempData["error"] = response.Message;
                    return RedirectToAction("MyCampaigns");
                }
                TempData["success"] = response.Message;
                return RedirectToAction("MyCampaigns");
            }
            TempData["error"] = "Error! Could not update campaign!";
            return RedirectToAction("MyCampaigns");
        }
        public IActionResult ContestantDetails(Guid candidatePositionId)
        {
            var response = _candidatePositionService.GetCandidatePositionById(candidatePositionId);
            if (response.Data == null)
            {
                TempData["error"] = response.Message;
                return RedirectToAction("GetAll");
            }
            ViewBag.PositionId = response.Data.PositionId;

            if (User.Identity.IsAuthenticated)
            {
                string? email = User.FindFirstValue(ClaimTypes.Email);
                if (email != null)
                {
                    var stuExists = _studentService.GetStudentByEmail(email);
                    if (stuExists.Data != null)
                    {
                        var electionId = response.Data.Position?.ElectionId;
                        var studentId = stuExists.Data.Id;
                        if (_electionService.StudentIsEligible(studentId, (Guid)electionId).Status)
                        {
                            var positionId = response.Data.PositionId;
                            if (!_voteService.VoteCastingInfoExists(studentId, positionId).Status)
                            {
                                ViewBag.CanVote = true;
                            }
                        }
                    }
                }
            }
            return View(response.Data);
        }
        //public IActionResult Vote()
        //{

        //}
        [Authorize(Roles = "Admin")]
        public IActionResult Update(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var response = _sessionService.GetAllSessions();
            var response2 = _ruleService.GetAllRules();
            var sessions = response.Data?.ToList();
            var rules = response2.Data?.ToList();
            ViewData["Session"] = new SelectList(sessions, "Id", "Name");
            ViewData["Rule"] = new SelectList(rules, "Id", "Name");
            var response3 = _electionService.GetUpdateElection((Guid)id);
            if (response3.Data == null)
            {
                TempData["error"] = response.Message;
                return NotFound();//
            }
            ViewBag.ElectionId = id;
            return View(response3.Data);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(Guid id, ElectionRequestModel request)
        {
            if (ModelState.IsValid)
            {
                request.Name = request.Name.ToLower();
                var response = _electionService.UpdateElection(id, request);
                if (response.Data == null)
                {
                    TempData["error"] = response.Message;
                    return RedirectToAction("GetAll");
                }
                TempData["success"] = response.Message;
                return RedirectToAction("GetAll");
            }
            TempData["error"] = "Error! Could not update election!";
            return RedirectToAction("GetAll");
        }
        //done
        public IActionResult Results(Guid electionId, int? pageNumber)
        {
            var response = _voteService.GetLiveResults(electionId);//work on it service implementation and view also
            if (response.Data == null)
            {
                TempData["error"] = response.Message;
                return RedirectToAction("GetAll");
            }
            ViewBag.ElectionId = electionId;
            int pageSize = 12;
            TempData["success"] = response.Message;//order by position

            return View(PaginatedList<CandidatePositionResponseModel>.Create(response.Data.ToList(), pageNumber ?? 1, pageSize));
        }
        [Authorize(Roles = "Student")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Vote(string email, Guid id)
        {
            // not needed
            var stuExists = _studentService.GetStudentByEmail(email);
            if (stuExists.Data == null)
            {
                TempData["error"] = stuExists.Message;
                return RedirectToAction("GetAll");
            }
            var contestant = _candidatePositionService.GetCandidatePositionById(id);
            if (contestant.Data == null)
            {
                TempData["error"] = contestant.Message;
                return RedirectToAction("GetAll");
            }
            var studentId = stuExists.Data.Id;
            var electionId = contestant.Data.Position?.ElectionId;
            if (!_electionService.StudentIsEligible(studentId, (Guid)electionId).Status)
            {
                TempData["error"] = "You are not eligible to vote in this election!!";
                return RedirectToAction("GetAll");
            }//
            var response = _voteService.CreateVoteCastingInfo(studentId, id);
            if (response.Data == null)
            {
                TempData["error"] = response.Message;
                return RedirectToAction("GetAll");
            }
            ViewBag.CanVote = false;
            ViewBag.PositionId = contestant.Data.PositionId;
            TempData["success"] = response.Message;//position id viewbag
            //or
            //contestant = _candidatePositionService.GetCandidatePositionById(id);
            //return View("ContestantDetails", contestant.Data);
            return RedirectToAction("ContestantDetails", new { candidatePositionId = contestant.Data.Id } );
        }
        [Authorize(Roles = "Student")]
        public IActionResult MyVotes(int? pageNumber)
        {
            string? email = User.FindFirstValue(ClaimTypes.Email);
            if (email != null)
            {
                var stuExists = _studentService.GetStudentByEmail(email);
                if (stuExists.Data != null)
                {
                    var studentId = stuExists.Data.Id;
                    var response = _voteService.GetAllVotesByStudent(studentId);
                    if (response.Data == null)
                    {
                        TempData["error"] = response.Message;
                        return RedirectToAction("GetAll");
                    } 
                    int pageSize = 25;
                    return View(PaginatedList<CandidatePositionResponseModel>.Create(response.Data.Select(c => c.CandidatePosition).ToList(), pageNumber ?? 1, pageSize));
                    
                }
            }
            TempData["error"] = "an error occurred!";
            return RedirectToAction("GetAll");
        }
        [Authorize(Roles = "Student")]
        public IActionResult MyCampaigns(int? pageNumber)
        {
            string? email = User.FindFirstValue(ClaimTypes.Email);
            if (email != null)
            {
                var stuExists = _studentService.GetStudentByEmail(email);
                if (stuExists.Data != null)
                {
                    var studentId = stuExists.Data.Id;
                    var response = _candidatePositionService.GetAllCandidatePositions();
                    if (response.Data == null)
                    {
                        TempData["error"] = response.Message;
                        return RedirectToAction("GetAll");
                    }
                    int pageSize = 25;
                    return View(PaginatedList<CandidatePositionResponseModel>.Create(response.Data.Where(c => c.Candidate?.StudentId == studentId).ToList(), pageNumber ?? 1, pageSize));
                }

            }
            TempData["error"] = "an error occurred!";
            return RedirectToAction("GetAll");
        } 
    }

}