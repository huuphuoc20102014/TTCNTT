using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TTCNTT.Efs.Entities;

namespace TTCNTT.Models
{
    public class ProductViewModel
    {
        public Product product { get; set; }
        public List<Product> listProduct { get; set; }
        public Category category { get; set; }
        public List<Category> listCategory { get; set; }
        public Menu menu { get; set; }
        public List<ProductComment> listProductComment { get; set; }
    }
}
