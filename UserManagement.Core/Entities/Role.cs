﻿using Microsoft.AspNetCore.Identity;

namespace UserManagement.Core.Entities
{
    public class Role : IdentityRole
    {
        public ICollection<UserRole> UserRoles { get; set; }
    }
}
