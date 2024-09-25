using System.Linq.Expressions;
using VotingApp.Models.Entities;

namespace VotingApp.Repository.Interfaces
{
    public interface ICandidatePositionRepository
    {
        void Create(CandidatePosition candidateposition);
        void Update(CandidatePosition candidateposition);
        CandidatePosition? Get(Expression<Func<CandidatePosition, bool>> predicate);
        bool Exists(Func<CandidatePosition, bool> predicate);
        ICollection<CandidatePosition> GetAll();
        ICollection<CandidatePosition> GetAllByIndex(Expression<Func<CandidatePosition, bool>> predicate);
        void Save();
    }
}
