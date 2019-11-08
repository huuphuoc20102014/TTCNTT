using System;
using System.Collections.Generic;

namespace ATAdmin.Efs.Entities
{
    public partial class Menu : AtBaseECommerceEntity
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string SlugName { get; set; }
        public bool AutoSlug { get; set; }
        public string ControlerName { get; set; }
        public string ActionName { get; set; }
        public string AnotherLink { get; set; }
        public string FkMenuId { get; set; }
        public bool IsAdminMenu { get; set; }
        public bool IsMainMenu { get; set; }
        public string ImageSlug { get; set; }
        public string IconSlug { get; set; }
        public string CssClass { get; set; }
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
