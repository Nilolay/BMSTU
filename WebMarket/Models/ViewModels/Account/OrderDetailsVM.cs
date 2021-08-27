using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebMarket.Models.Data;

namespace WebMarket.Models.ViewModels.Account
{
    public class OrderDetailsVM
    {
        public OrderDetailsVM()
        {

        }
        public OrderDetailsVM(OrderDetailsDTO row)
        {
            Id = row.Id;
            OrderId = row.Id;
            UserId = row.UserId;
            ProductId = row.ProductId;
            Quantity = row.Quantity;
        }
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}