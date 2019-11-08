using System;
using System.Collections.Generic;

namespace ATAdmin.Efs.Entities
{
    public partial class People : AtBaseECommerceEntity
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime BirthDay { get; set; }
        public string Job { get; set; }
        public string JobIntroduction { get; set; }
        public string Phone { get; set; }
        public string Gmail { get; set; }
        public string Img { get; set; }
        public int RowStatus { get; set; }
        public byte[] RowVersion { get; set; }
    }
}
