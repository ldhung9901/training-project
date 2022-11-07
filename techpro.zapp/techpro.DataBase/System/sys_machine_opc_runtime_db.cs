using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace techpro.DataBase.System
{
    [Table("sys_machine_opc_runtime")]
    public class sys_machine_opc_runtime_db
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [MaxLength(128)]
        public string id { get; set; }
        [MaxLength(500)]
        public string id_machine_opc { get; set; }
        [MaxLength(500)]
        public string id_machine { get; set; }
        public int? chart_type { get; set; }
        //1 number, 2 string
        public int? type_value { get; set; }
        [MaxLength(500)]
        public string opc_node_id { get; set; }
        [MaxLength(500)]
        public string value { get; set; }
        public int? time_receive { get; set; }
    }

}
