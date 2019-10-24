using System;
using System.Collections.Generic;

namespace TTCNTT.Efs.Entities
{
    public partial class Faq
    {
        public string Id { get; set; }
        public string Faqquestion { get; set; }
        public string Faqreply { get; set; }
        public int ShowIndex { get; set; }
        public bool Active { get; set; }
        public byte[] RowVersion { get; set; }
        public int RowStatus { get; set; }
    }
}
