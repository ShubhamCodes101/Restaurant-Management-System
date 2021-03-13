using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RestoManageSystem.Models
{
    public class admincred
    {
        [Required]
        public string Userid { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

    }
}