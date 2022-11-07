using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace techpro.DataBase.System
{
    [Table("sys_vendor_item_history")]
    public class sys_vendor_item_history_db
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public long id { get; set; }
        [MaxLength(500)]
        public string id_supplier { get; set; }
        [MaxLength(500)]
        public string id_item { get; set; }
        [MaxLength(500)]
        public string id_unit { get; set; }
        [MaxLength(500)]
        public string id_unit_main { get; set; }
        public long? id_specification { get; set; }
        [MaxLength(128)]
        public string create_by { get; set; }
        [Column(TypeName="DateTime")]
        public DateTime? create_date { get; set; }
        [MaxLength(128)]
        public string update_by { get; set; }
        [Column(TypeName="DateTime")]
        public DateTime? update_date { get; set; }
        [Column(TypeName = "decimal(18, 4)")]
        public decimal price_item { get; set; }   
        [Column(TypeName = "decimal(18, 4)")]     
        public decimal min_stock_order { get; set; }
        [Column(TypeName = "decimal(18, 4)")]
        public decimal delivery_time { get; set; }
        [Column(TypeName = "decimal(18, 4)")]
        public decimal? conversion_factor { get; set; }
        [MaxLength(500)]
        public string action { get; set; }
        [MaxLength(500)]
        public string note { get; set; }
    }
}
