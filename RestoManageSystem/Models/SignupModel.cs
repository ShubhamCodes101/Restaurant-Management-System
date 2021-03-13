using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RestoManageSystem.Models
{
    public class SignupModel
    {
        [Required]
        public string Userid { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        public string Usercity { get; set; }

        [Required]
        public string Userphno { get; set; }
    }
}