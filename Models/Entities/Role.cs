﻿namespace VotingApp.Models.Entities
{
    public class Role
    {
        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public ICollection<User> Users { get; } = [];

    }
}
