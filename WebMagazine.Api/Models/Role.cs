﻿namespace WebMagazine.Api.Models
{
    public class Role
    {
        public int RoleID { get; set; }
        public string Name { get; set; }

        public ICollection<User> Users { get; set; }
    }
}
