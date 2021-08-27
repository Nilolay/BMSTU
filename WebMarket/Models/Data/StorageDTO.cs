using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebMarket.Models.Data
{
    [Table("tblStorage")]
    public class StorageDTO
    {
        [Key]
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Stored { get; set; }

        [ForeignKey("ProductId")]
        public virtual ProductDTO Products { get; set; }
    }
}