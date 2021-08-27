using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebMarket.Models.Data
{
    [Table("tblRoles")]
    public class RoleDTO
    {
        [Key]
        public int id { get; set; }
        public string Name { get; set; }
    }
}