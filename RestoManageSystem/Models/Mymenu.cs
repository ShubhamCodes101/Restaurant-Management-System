using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RestoManageSystem.Models
{
    public class Mymenu
    {
        public string Itemsname { get; set; }
        public Nullable<int> price { get; set; }
        public string Itemimage { get; set; }
        public string Foodtype { get; set; }
    }
}