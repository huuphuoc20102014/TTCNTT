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
        public string SlugName { get; set; }
        public bool AutoSlug { get; set; }
        public string FkProductId { get; set; }
        public string SpecificationHtml { get; set; }
        public string ShortDescriptionHtml { get; set; }
        public string LongDescriptionHtml { get; set; }
        public string Sku { get; set; }
        public string Color { get; set; }
        public string Size { get; set; }
        public string Material { get; set; }
        public string Style { get; set; }
        public double? Price { get; set; }
        public string Ccy { get; set; }
        public string Country { get; set; }
        public string Producer { get; set; }
        public string Status { get; set; }
        public string ImageSlug { get; set; }
        public int? Rating { get; set; }
        public int? CountView { get; set; }
        public bool IsService { get; set; }
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
        public int CountComment { get; set; }
        public int CountReply { get; set; }

        public virtual Category FkProduct { get; set; }
        public virtual ICollection<ProductComment> ProductComment { get; set; }
        public virtual ICollection<ProductImage> ProductImage { get; set; }
    }
}
