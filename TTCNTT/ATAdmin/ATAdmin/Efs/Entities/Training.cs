using System;
using System.Collections.Generic;

namespace ATAdmin.Efs.Entities
{
    public partial class Training : AtBaseECommerceEntity
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Slug_Name { get; set; }
        public string Description { get; set; }
    }
}
