using System;
using System.Collections.Generic;

namespace ATAdmin.Efs.Entities
{
    public partial class AspNetUserClaims : AtBaseECommerceEntity
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }

        public virtual AspNetUsers User { get; set; }
    }
}
