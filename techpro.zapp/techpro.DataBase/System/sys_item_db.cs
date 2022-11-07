using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace techpro.DataBase.System
{
    [Table("sys_item")]
    public class sys_item_db
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [MaxLength(128)]
        public string id { get; set; }
        [Column(TypeName = "decimal(18, 4)")]
        public decimal? price { get; set; }
        [MaxLength(128)]
        public string id_item_type { get; set; }
        [MaxLength(200)]
        public string name { get; set; }
        [MaxLength(500)]
        public string note { get; set; }
        [MaxLength(500)]
        public string pathImg { get; set; }
        public int? type { get; set; }

        [MaxLength(128)]
        public string id_unit { get; set; }
        [MaxLength(128)]
        public string id_cost_type { get; set; }
        public int? status_del { get; set; }
        [MaxLength(128)]
        public string create_by { get; set; }
        [Column(TypeName="DateTime")]
        public DateTime? create_date { get; set; }
        [MaxLength(128)]
        public string update_by { get; set; }
        [Column(TypeName="DateTime")]
        public DateTime? update_date { get; set; }
        public int? has_bom { get; set; }
        public int? has_specification { get; set; }
        [MaxLength(200)]
        public string code_item { get; set; }
        [MaxLength(200)]
        public string description { get; set; }
     
    }

}
