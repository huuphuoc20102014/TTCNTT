using System;
using System.Collections.Generic;

namespace ATAdmin.Efs.Entities
{
    public partial class AspNetUserTokens : AtBaseECommerceEntity
    {
        public string UserId { get; set; }
        public string LoginProvider { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }

        public virtual AspNetUsers User { get; set; }
    }
}
