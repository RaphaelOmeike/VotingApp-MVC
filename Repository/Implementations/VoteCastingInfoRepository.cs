using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using VotingApp.Context;
using VotingApp.Models.Entities;
using VotingApp.Repository.Interfaces;

namespace VotingApp.Repository.Implementations
{
    public class VoteCastingInfoRepository : IVoteCastingInfoRepository
    {
        private readonly ApplicationDbContext _context;
        public VoteCastingInfoRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public void Create(VoteCastingInfo voteCastingInfo)
        {
            _context.VoteCastingInfos.Add(voteCastingInfo);
        }

        public bool Exists(Func<VoteCastingInfo, bool> predicate)
        {
            return _context.VoteCastingInfos.Include(c => c.CandidatePosition).Any(predicate);
        }

        public VoteCastingInfo? Get(Expression<Func<VoteCastingInfo, bool>> predicate)
        {
            var votecastinginfo = _context.VoteCastingInfos.Include(c => c.CandidatePosition).Include(c => c.Student).FirstOrDefault(predicate);
            return votecastinginfo;
        }

        public ICollection<VoteCastingInfo> GetAll()
        {
            var votecastinginfos = _context.VoteCastingInfos.Include(c => c.CandidatePosition).Include(c => c.Student).ToList();
            return votecastinginfos;
        }

        public ICollection<VoteCastingInfo> GetAllByIndex(Expression<Func<VoteCastingInfo, bool>> predicate)
        {
            var votecastinginfos = _context.VoteCastingInfos.Include(c => c.Student).Include(c => c.CandidatePosition).ThenInclude(c => c.Position).ThenInclude(c => c.Election).Include(c => c.CandidatePosition).ThenInclude(c => c.Votes).Include(c => c.CandidatePosition).ThenInclude(c => c.Candidate).ThenInclude(c => c.Student).ThenInclude(c => c.Course).Where(predicate).ToList();
            return votecastinginfos;
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(VoteCastingInfo voteCastingInfo)
        {
            _context.Update(voteCastingInfo);
        }
    }
}
