using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace techpro.DataBase.System
{
    [Table("sys_working_time_detail_config")]
    public class sys_working_time_detail_config_db
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public long id { get; set; }
        [MaxLength(128)]
        public string create_by { get; set; }
        [MaxLength(500)]
        public string id_group { get; set; }
        [Column(TypeName = "DateTime")]
        public DateTime? create_date { get; set; }
        [MaxLength(128)]
        public int? shift_1 { get; set; }
        [MaxLength(128)]
        public int? shift_2 { get; set; }
        public bool? is_off { get; set; }
        [Column(TypeName = "DateTime")]
        public DateTime? estimate_check_date { get; set; }
        [MaxLength(128)]
        public string update_by { get; set; }
        [Column(TypeName = "DateTime")]
        public DateTime? update_date { get; set; }
        [MaxLength(500)]
        public string note { get; set; }

    }
}
