using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ManagerSystem.Entities
{
    public class JobEntity : BaseEntity
    {
        public int order_id;

        [Required]
        [ForeignKey("order_id")]
        public OrderEntity order;


    }
}