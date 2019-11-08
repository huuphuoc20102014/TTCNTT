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
        public string Style { get; set; }
        public int RowStatus { get; set; }
        public byte[] RowVersion { get; set; }
        public string ImageSlug { get; set; }

        public virtual SettingType StyleNavigation { get; set; }
    }
}
