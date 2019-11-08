using System;
using System.Collections.Generic;

namespace TTCNTT.Efs.Entities
{
    public partial class Contact
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string FkCourseId { get; set; }
        public int? CourseMember { get; set; }
        public bool IsRead { get; set; }
        public string Note { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public byte[] RowVersion { get; set; }
        public int RowStatus { get; set; }
        public string Adress { get; set; }
        public string Link { get; set; }

        public virtual Course FkCourse { get; set; }
    }
}
