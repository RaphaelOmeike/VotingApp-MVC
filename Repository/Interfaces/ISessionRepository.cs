using System.Linq.Expressions;
using VotingApp.Models.Entities;

namespace VotingApp.Repository.Interfaces
{
    public interface ISessionRepository
    {
        void Create(Session session);
        void Update(Session session);
        Session? Get(Expression<Func<Session, bool>> predicate);
        bool Exists(Func<Session, bool> predicate);
        ICollection<Session> GetAll();
        ICollection<Session> GetAllByIndex(Expression<Func<Session, bool>> predicate);
        void Save();
    }
}
