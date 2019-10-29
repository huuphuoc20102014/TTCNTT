using System;
using System.Collections.Generic;

namespace TTCNTT.Efs.Entities
{
    public partial class NewsComment
    {
        public string Id { get; set; }
        public string FkNewsId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Comment { get; set; }
        public int? Rating { get; set; }
        public bool? IsRead { get; set; }
        public string Note { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public byte[] RowVersion { get; set; }
        public int RowStatus { get; set; }
        public int? CountReply { get; set; }

        public virtual News FkNews { get; set; }
    }
}
