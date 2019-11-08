using ATAdmin.Efs.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ATAdmin.Models
{
    public class CategoryCreate
    {
        public string Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Code { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        [Required]
        [StringLength(100)]
        public string Slug { get; set; }
        public bool AutoSlug { get; set; }
        [StringLength(50)]
        public string FkParentId { get; set; }
        [StringLength(500)]
        [DataType(DataType.MultilineText)]
        public string Note { get; set; }

        [ForeignKey("FkParentId")]
        public virtual CategoryCreate FkParent { get; set; }
    }
}
