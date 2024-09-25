using System.Linq.Expressions;
using VotingApp.Models.Entities;

namespace VotingApp.Repository.Interfaces
{
    public interface ICandidateRepository
    {
        void Create(Candidate candidate);
        void Update(Candidate candidate);
        Candidate? Get(Expression<Func<Candidate, bool>> predicate);
        bool Exists(Func<Candidate, bool> predicate);
        ICollection<Candidate> GetAll();
        ICollection<Candidate> GetAllByIndex(Expression<Func<Candidate, bool>> predicate);
        void Save();
    }
}
