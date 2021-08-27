using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebMarket.Models.Data;

namespace WebMarket.Models.ViewModels.Account
{
    public class AccountVM
    {
        public AccountVM()
        {

        }
        public int UserId { get; set; }
        public decimal Ammount { get; set; }
    }
}