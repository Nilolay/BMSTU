using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMarket.Models.Data;

namespace WebMarket.Models.ViewModels.Pages
{
    public class SidebarVM
    {
        public SidebarVM()
        {

        }

        public SidebarVM(SidebarDTO row)
        {
            Id = row.id;
            Body = row.body;
        }
        public int Id { get; set; }

        [AllowHtml]
        public string Body { get; set; }
    }
}