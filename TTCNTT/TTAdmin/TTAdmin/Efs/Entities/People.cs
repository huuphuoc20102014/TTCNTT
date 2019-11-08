using System;
using System.Collections.Generic;

namespace TTAdmin.Efs.Entities
{
    public partial class People
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime BirthDay { get; set; }
        public string Job { get; set; }
        public string JobIntroduction { get; set; }
        public string Phone { get; set; }
        public string Gmail { get; set; }
        public string Img { get; set; }
    }
}
