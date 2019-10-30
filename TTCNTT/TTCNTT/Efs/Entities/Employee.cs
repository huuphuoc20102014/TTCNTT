using System;
using System.Collections.Generic;

namespace TTCNTT.Efs.Entities
{
    public partial class Employee
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Slug_Name { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Address { get; set; }
        public string Specialize { get; set; }
        public string Fk_EmplyeeId { get; set; }
        public string ImageSlug { get; set; }
        public string ShortDescription_Html { get; set; }
        public string LongDescription_Html { get; set; }
        public string Note { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public byte[] RowVersion { get; set; }
        public int RowStatus { get; set; }

        public virtual EmployeeType Fk_Emplyee { get; set; }
    }
}
