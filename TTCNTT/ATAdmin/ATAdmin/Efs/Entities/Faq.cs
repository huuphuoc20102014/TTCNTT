using System;
using System.Collections.Generic;

namespace ATAdmin.Efs.Entities
{
    public partial class FAQ : AtBaseECommerceEntity
    {
        public string ID { get; set; }
        public string FAQQuestion { get; set; }
        public string FAQReply { get; set; }
        public int ShowIndex { get; set; }
        public bool Active { get; set; }
        public byte[] RowVersion { get; set; }
        public int RowStatus { get; set; }
    }
}
