using System;
using System.Collections.Generic;

namespace TTAdmin.Efs.Entities
{
    public partial class AboutUs
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Slug_Title { get; set; }
        public bool AutoSlug { get; set; }
        public string Name { get; set; }
        public string Skill { get; set; }
        public string ShortDescription_Html { get; set; }
        public string LongDescription_Html { get; set; }
        public string ImageSlug { get; set; }
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
    }
}
