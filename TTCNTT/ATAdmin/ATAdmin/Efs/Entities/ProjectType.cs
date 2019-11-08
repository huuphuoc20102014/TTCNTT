using System;
using System.Collections.Generic;

namespace ATAdmin.Efs.Entities
{
    public partial class ProjectType : AtBaseECommerceEntity
    {
        public ProjectType()
        {
            Project = new HashSet<Project>();
        }

        public string Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string SlugName { get; set; }
        public bool AutoSlug { get; set; }
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

        public virtual ICollection<Project> Project { get; set; }
    }
}
