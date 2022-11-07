using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace techpro.DataBase.System
{
    [Table("sys_item_bom_config")]
    public class sys_item_bom_config_db
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public long id { get; set; }
        [MaxLength(500)]
        public string id_item { get; set; }
        [MaxLength(128)]
        public string config_code { get; set; }
        public long? id_specification { get; set; }
        public long? status_del { get; set; }
        [MaxLength(500)]
        public string id_unit { get; set; }
        [MaxLength(128)]
        public string create_by { get; set; }
        [Column(TypeName="DateTime")]
        public DateTime? create_date { get; set; }
        [MaxLength(128)]
        public string update_by { get; set; }
        [Column(TypeName="DateTime")]
        public DateTime? update_date { get; set; }
        [Column(TypeName="DateTime")]
        public DateTime? aplly_date { get; set; }
        [MaxLength(500)]
        public string note { get; set; }
        [MaxLength(500)]
        public string name { get; set; }
        public bool? status_use { get; set; }

    }
}
