//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RestoManageSystem.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Menu
    {
        public int Id { get; set; }
        public string Itemsname { get; set; }
        public Nullable<int> price { get; set; }
        public byte[] Itemimage { get; set; }
        public string Foodtype { get; set; }
        public string Rname { get; set; }
    
        public virtual Restaurant Restaurant { get; set; }
    }
}
