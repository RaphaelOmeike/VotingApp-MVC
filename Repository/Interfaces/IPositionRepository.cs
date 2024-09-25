using System.Linq.Expressions;
using VotingApp.Models.Entities;

namespace VotingApp.Repository.Interfaces
{
    public interface IPositionRepository
    {
        void Create(Position position);
        void Update(Position position);
        Position? Get(Expression<Func<Position, bool>> predicate);
        bool Exists(Func<Position, bool> predicate);
        ICollection<Position> GetAll();
        ICollection<Position> GetAllByIndex(Expression<Func<Position, bool>> predicate);
        void Save();
    }
}
