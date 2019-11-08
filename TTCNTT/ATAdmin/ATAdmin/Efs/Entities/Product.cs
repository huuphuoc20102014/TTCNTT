using System;
using System.Collections.Generic;

namespace ATAdmin.Efs.Entities
{
    public partial class Product : AtBaseECommerceEntity
    {
        public Product()
        {
            ProductComment = new HashSet<ProductComment>();
            ProductImage = new HashSet<ProductImage>();
        }

        public string Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Slug_Name { get; set; }
        public bool AutoSlug { get; set; }
        public string FkProductId { get; set; }
        public string ShortDescription_Html { get; set; }
        public string LongDescription_Html { get; set; }
        public string ImageSlug { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public byte[] RowVersion { get; set; }
        public int RowStatus { get; set; }
        public int? CountComment { get; set; }
        public int? CountReply { get; set; }
        public int? CountView { get; set; }
        public string Tags { get; set; }
        public string KeyWord { get; set; }
        public string MetaData { get; set; }
        public string Note { get; set; }

        public virtual Category FkProduct { get; set; }
        public virtual ICollection<ProductComment> ProductComment { get; set; }
        public virtual ICollection<ProductImage> ProductImage { get; set; }
    }
}
