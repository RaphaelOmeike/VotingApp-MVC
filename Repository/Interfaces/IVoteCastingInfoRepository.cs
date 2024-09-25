using System.Linq.Expressions;
using VotingApp.Models.Entities;

namespace VotingApp.Repository.Interfaces
{
    public interface IVoteCastingInfoRepository
    {
        void Create(VoteCastingInfo voteCastingInfo);
        void Update(VoteCastingInfo voteCastingInfo);
        VoteCastingInfo? Get(Expression<Func<VoteCastingInfo, bool>> predicate);
        bool Exists(Func<VoteCastingInfo, bool> predicate);
        ICollection<VoteCastingInfo> GetAll();
        ICollection<VoteCastingInfo> GetAllByIndex(Expression<Func<VoteCastingInfo, bool>> predicate);
        void Save();
    }
}
