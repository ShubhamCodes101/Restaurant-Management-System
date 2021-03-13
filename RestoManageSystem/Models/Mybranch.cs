using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RestoManageSystem.Models
{
    public class Mybranch
    {
        public int branchid { get; set; }
        public string bcity { get; set; }
        public string Location { get; set; }
        public Nullable<long> bphonenum { get; set; }
        public string bimage { get; set; }
        public string Rname { get; set; }
    }
}