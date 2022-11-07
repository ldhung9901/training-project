using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace techpro.DataBase.System
{
    [Table("sys_item_bom_quality")]
    public class sys_item_bom_quality_db
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        [MaxLength(128)]
        public string id_item { get; set; }
        public long id_item_bom_config { get; set; }
        [MaxLength(128)]
        public string id_phase { get; set; }
        [MaxLength(500)]
        public string content { get; set; }
        [MaxLength(500)]
        public string description { get; set; }
        [MaxLength(200)]
        public string type_evaluate { get; set; }
        [Column(TypeName = "decimal(18, 4)")]
        public decimal? number_evaluate { get; set; }
        [Column(TypeName = "decimal(18, 4)")]
        public decimal? error_range_start { get; set; }
        [Column(TypeName = "decimal(18, 4)")]
        public decimal? error_range_end { get; set; }
        public int status_del { get; set; }

        [MaxLength(200)]
        public string create_by { get; set; }
        [Column(TypeName = "DateTime")]
        public DateTime? create_date { get; set; }
        [MaxLength(200)]
        public string update_by { get; set; }
        [Column(TypeName = "DateTime")]
        public DateTime? update_date { get; set; }

    }
}
