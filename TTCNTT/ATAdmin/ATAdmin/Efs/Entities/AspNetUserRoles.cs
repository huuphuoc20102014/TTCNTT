using System;
using System.Collections.Generic;

namespace ATAdmin.Efs.Entities
{
    public partial class AspNetUserRoles : AtBaseECommerceEntity
    {
        public string UserId { get; set; }
        public string RoleId { get; set; }

        public virtual AspNetRoles Role { get; set; }
        public virtual AspNetUsers User { get; set; }
    }
}
