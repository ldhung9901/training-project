using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace techpro.DataBase.System
{
    [Table("sys_item_min_max_stock")]
    public class sys_item_min_max_stock_db
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public long id { get; set; }
        [MaxLength(500)]
        public string id_item { get; set; }
        [MaxLength(500)]
        public string id_warehouse { get; set; }
        [Column(TypeName = "decimal(18, 4)")]
        public decimal? min_stock { get; set; }
        [Column(TypeName = "decimal(18, 4)")]
        public decimal? max_stock { get; set; }
    }
   
}
