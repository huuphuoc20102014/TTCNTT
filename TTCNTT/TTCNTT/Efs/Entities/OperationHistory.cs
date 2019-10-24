using System;
using System.Collections.Generic;

namespace TTCNTT.Efs.Entities
{
    public partial class OperationHistory
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string HistoryDescription { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
