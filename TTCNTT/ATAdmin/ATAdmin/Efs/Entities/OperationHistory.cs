using System;
using System.Collections.Generic;

namespace ATAdmin.Efs.Entities
{
    public partial class OperationHistory : AtBaseECommerceEntity
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string HistoryDescription { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
