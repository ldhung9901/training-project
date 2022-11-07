using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace techpro.DataBase.System
{
    [Table("sys_item_bom")]
    public class sys_item_bom_db
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public long id { get; set; }
        [MaxLength(500)]
        public string id_item { get; set; }
        public long? id_item_bom_config { get; set; }
        [MaxLength(500)]
        public string id_item_bom { get; set; }
        [MaxLength(500)]
        public string id_phase { get; set; }
        public long? id_specification { get; set; }
        [MaxLength(500)]
        public string id_unit { get; set; }
        [MaxLength(500)]
        public string id_unit_main { get; set; }
        [Column(TypeName = "decimal(18, 4)")]
        public decimal? quota { get; set; }       
        [Column(TypeName = "decimal(18, 4)")] 
        public decimal? quota_main { get; set; }
        [Column(TypeName = "decimal(18, 4)")]
        public decimal? conversion_factor { get; set; }
        
        [MaxLength(128)]
        public string create_by { get; set; }
        [Column(TypeName="DateTime")]
        public DateTime? create_date { get; set; }
        [MaxLength(128)]
        public string update_by { get; set; }
        [Column(TypeName="DateTime")]
        public DateTime? update_date { get; set; }
        [MaxLength(500)]
        public string note { get; set; }
        public int? status_del { get; set; }
        
    }
}
