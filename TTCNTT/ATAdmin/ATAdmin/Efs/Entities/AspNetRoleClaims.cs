using System;
using System.Collections.Generic;

namespace ATAdmin.Efs.Entities
{
    public partial class AspNetRoleClaims : AtBaseECommerceEntity
    {
        public int Id { get; set; }
        public string RoleId { get; set; }
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }

        public virtual AspNetRoles Role { get; set; }
    }
}
