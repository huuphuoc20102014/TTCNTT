using System;
using System.Collections.Generic;

namespace ATAdmin.Efs.Entities
{
    public partial class View_Users_Roles : AtBaseECommerceEntity
    {
        public string TenQuyen { get; set; }
        public string TenNguoiDung { get; set; }
        public string IdUser { get; set; }
        public string IdRole { get; set; }
    }
}
