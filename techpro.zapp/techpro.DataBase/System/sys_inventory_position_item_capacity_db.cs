using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace techpro.DataBase.System
{
    [Table("sys_inventory_position_item_capacity")]
    public class sys_inventory_position_item_capacity_db
    {
        
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public long id { get; set; }
        [MaxLength(500)]
        public string id_warehouse { get; set; }
        [MaxLength(500)]
        public string id_position { get; set; }
        [MaxLength(500)]
        public string id_item { get; set; }
        [MaxLength(500)]
        public string id_unit { get; set; }
        [MaxLength(500)]
        public string id_unit_main { get; set; }
        public long? id_specification { get; set; }
        [Column(TypeName = "decimal(18, 4)")]
        public decimal? conversion_factor { get; set; }
        public int? type_add { get; set; }
        [MaxLength(128)]
        public string create_by { get; set; }
        [Column(TypeName="DateTime")]
        public DateTime? create_date { get; set; }
        [MaxLength(128)]
        public string update_by { get; set; }
        [Column(TypeName="DateTime")]
        public DateTime? update_date { get; set; }
        [Column(TypeName = "decimal(18, 4)")]
        public decimal? min_stock { get; set; }
        [Column(TypeName = "decimal(18, 4)")]
        public decimal? max_stock { get; set; }
    }
}
