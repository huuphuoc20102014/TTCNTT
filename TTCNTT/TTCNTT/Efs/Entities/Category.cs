using System;
using System.Collections.Generic;

namespace TTCNTT.Efs.Entities
{
    public partial class Category
    {
        public Category()
        {
            InverseFkCategory = new HashSet<Category>();
            Product = new HashSet<Product>();
        }

        public string Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Slug_Name { get; set; }
        public bool AutoSlug { get; set; }
        public string FkCategoryId { get; set; }
        public int SortIndex { get; set; }
        public string Note { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public byte[] RowVersion { get; set; }
        public int RowStatus { get; set; }
        public int? CountChild { get; set; }
        public int? CountProduct { get; set; }

        public virtual Category FkCategory { get; set; }
        public virtual ICollection<Category> InverseFkCategory { get; set; }
        public virtual ICollection<Product> Product { get; set; }
    }
}
