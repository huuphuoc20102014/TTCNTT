using System;
using System.Collections.Generic;

namespace TTCNTT.Efs.Entities
{
    public partial class Course
    {
        public Course()
        {
            Contact = new HashSet<Contact>();
        }

        public string Id { get; set; }
        public string FkCourseTypeId { get; set; }
        public string Name { get; set; }
        public string Slug_Name { get; set; }
        public bool AutoSlug { get; set; }
        public string ImageSlug { get; set; }
        public string ShortDescription_Html { get; set; }
        public string LongDescription_Html { get; set; }
        public string Tags { get; set; }
        public string KeyWord { get; set; }
        public string MetaData { get; set; }
        public string Note { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public byte[] RowVersion { get; set; }
        public int RowStatus { get; set; }

        public virtual CourseType FkCourseType { get; set; }
        public virtual ICollection<Contact> Contact { get; set; }
    }
}
