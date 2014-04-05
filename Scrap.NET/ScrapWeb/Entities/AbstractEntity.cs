using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ScrapWeb.Entities
{
    public abstract class AbstractEntity
    {
        [Key, Column(Order=1)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        public DateTime creationDate { get; set; }

        public DateTime updatedDate { get; set; }

        public String createdBy { get; set; }

        public String updatedBy { get; set; }
    }
}