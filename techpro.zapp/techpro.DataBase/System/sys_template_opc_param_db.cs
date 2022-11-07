using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace techpro.DataBase.System
{
    [Table("sys_template_opc_param")]
    public class sys_template_opc_param_db
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [MaxLength(128)]
        public string id { get; set; }
        [MaxLength(500)]
        public string id_template_opc { get; set; }
        [MaxLength(500)]
        public string name { get; set; }
        public int? chart_type { get; set; }
        [MaxLength(500)]
        public string opc_node_id { get; set; }
        public int? interval_second { get; set; }
        public int? keep_record { get; set; }
        public int? status_del { get; set; }
        [MaxLength(128)]
        public string update_by { get; set; }
        [Column(TypeName = "DateTime")]
        public DateTime? update_date { get; set; }
        [Column(TypeName = "DateTime")]
        public DateTime? create_date { get; set; }
        [MaxLength(128)]
        public string create_by { get; set; }
        [MaxLength(500)]
        public string note { get; set; }

    }

}
