using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace techpro.DataBase.System
{
    [Table("sys_item_specification")]
    public class sys_item_specification_db
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public long id { get; set; }
        [MaxLength(500)]
        public string id_item { get; set; }
        [MaxLength(500)]
        public string name { get; set; }
        [MaxLength(500)]
        public string note { get; set; }
        [MaxLength(500)]
        public string long_ { get; set; }
        [MaxLength(500)]
        public string wide { get; set; }
        [MaxLength(500)]
        public string high { get; set; }
        [MaxLength(500)]
        public string outside_radius { get; set; }
        [MaxLength(500)]
        public string inside_radius { get; set; }
        [MaxLength(500)]
        public string other_parameter { get; set; }
        [MaxLength(128)]
        public string create_by { get; set; }
        [Column(TypeName="DateTime")]
        public DateTime? create_date { get; set; }
        [MaxLength(128)]
        public string update_by { get; set; }
        [Column(TypeName="DateTime")]
        public DateTime? update_date { get; set; }
        [Column(TypeName = "decimal(18, 4)")]
        public decimal? conversion_factor { get; set; }
        [MaxLength(500)]
        public string id_unit { get; set; }
        public int? status_del { get; set; }


    }
}
