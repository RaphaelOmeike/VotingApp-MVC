using System.ComponentModel.DataAnnotations.Schema;
using VotingApp.Models.Enums;

namespace VotingApp.Models.Entities
{
    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public string RoleId { get; set; } = null!;
        [ForeignKey("RoleId")]
        public Role Role { get; set; } = null!;
        public bool IsDeleted { get; set; }
    }
    //role class is needed
    //1-1 relationship and role repository roleid role role  listuser> under that role
}
