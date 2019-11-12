using System;
using System.Collections.Generic;

namespace ATAdmin.Efs.Entities
{
    public partial class Setting : AtBaseECommerceEntity
    {
        public string Id { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
        public bool? IsManual { get; set; }
        public string Id2 { get; set; }
        public Int32 RowStatus { get; set; }
        public byte[] RowVersion { get; set; }
        public string ImageSlug { get; set; }
    }
}
