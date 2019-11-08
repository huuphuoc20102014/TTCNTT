using System;
using System.Collections.Generic;

namespace ATAdmin.Efs.Entities
{
    public partial class TableVersion : AtBaseECommerceEntity
    {
        public string Id { get; set; }
        public DateTime LastModify { get; set; }
        public byte[] RowVersion { get; set; }
    }
}
