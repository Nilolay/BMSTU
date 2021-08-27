using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebMarket.Models.Data;

namespace WebMarket.Models.ViewModels.Shop
{
    public class CategoryVM
    {
        public CategoryVM()
        {

        }

        public CategoryVM(CategoryDTO row)
        {
            id = row.id;
            Name = row.Name;
            Slug = row.Slug;
            Sorting = row.Sorting;
            
        }
        public int id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public int Sorting { get; set; }

    }
}