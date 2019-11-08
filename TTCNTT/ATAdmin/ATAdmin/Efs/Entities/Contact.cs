using System;
using System.Collections.Generic;

namespace ATAdmin.Efs.Entities
{
    public partial class Contact : AtBaseECommerceEntity
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public bool IsRead { get; set; }
        public string FkProductCommentId { get; set; }
        public string Note { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public byte[] RowVersion { get; set; }
        public int RowStatus { get; set; }
        public string Adress { get; set; }
        public string Link { get; set; }
    }
}
