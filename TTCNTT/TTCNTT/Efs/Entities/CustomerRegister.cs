using System;
using System.Collections.Generic;

namespace TTCNTT.Efs.Entities
{
    public partial class CustomerRegister
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public byte[] RowVersion { get; set; }
        public int RowStatus { get; set; }
    }
}
