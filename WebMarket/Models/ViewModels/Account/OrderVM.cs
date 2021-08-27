using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using WebMarket.Models.Data;

namespace WebMarket.Models.ViewModels.Account
{
    
    public class OrderVM
    {
        public OrderVM()
        {

        }
        public OrderVM(OrderDTO row)
        {
            OrderId = row.OrderId;
            UserId = row.UserId;
            CreatedAt = row.CreatedAt;
        }
        public int OrderId { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }

    }
}