using System;
using System.Collections.Generic;

namespace ATAdmin.Efs.Entities
{
    public partial class ProductImage : AtBaseECommerceEntity
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string SlugName { get; set; }
        public bool AutoSlug { get; set; }
        public string FkProductId { get; set; }
        public string Extension { get; set; }
        public string Description { get; set; }
        public int SortIndex { get; set; }
        public bool IsYoutube { get; set; }
        public string YoutubeLink { get; set; }
        public string Thumbnail { get; set; }
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

        public virtual Product FkProduct { get; set; }
    }
}
