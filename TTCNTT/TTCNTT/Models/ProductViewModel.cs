﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TTCNTT.Efs.Entities;
using TTCNTT.Helpers;

namespace TTCNTT.Models
{
    public class ProductViewModel
    {
        public Product product { get; set; }
        public List<Product> listProduct { get; set; }
        public Category category { get; set; }
        public List<Category> listCategory { get; set; }
        public List<ProductComment> listProductComment { get; set; }
        public SettingHelper setting { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Content { get; set; }
        public string fkProductId { get; set; }
    }
}
