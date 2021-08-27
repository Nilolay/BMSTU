using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebMarket.Models.Data
{
    [Table("tblAccount")]
    public class AccountDTO
    {
        [Key]
        public int AccountId { get; set; }
        public int UserId { get; set; }
        public decimal Ammount { get; set; }

        [ForeignKey("UserId")]
        public virtual UserDTO Users { get; set; }
    }
}