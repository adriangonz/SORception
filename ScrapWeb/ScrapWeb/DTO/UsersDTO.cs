using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ScrapWeb.DTO
{
    public class UserRegisterDTO
    {
        [Required]
        public string username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string password { get; set; }
    }

    public class UserInfoDTO
    {
        [Required]
        public string id { get; set; }

        [Required]
        public string username { get; set; }

        [Required]
        public Boolean isAdmin { get; set; }
    }
}