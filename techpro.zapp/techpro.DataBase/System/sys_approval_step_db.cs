using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace techpro.DataBase.System
{
    [Table("sys_approval_step")]
    public class sys_approval_step_db
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public long id { get; set; }
        [MaxLength(500)]
        public string id_approval_config { get; set; }
        [MaxLength(500)]
        public string id_approval { get; set; }
        [MaxLength(500)]
        public string user_id { get; set; }
        [MaxLength(500)]
        public string name { get; set; }
        [MaxLength(500)]
        public string note { get; set; }
        [Column(TypeName = "decimal(18, 4)")]
        public decimal? duration_hours { get; set; }
        public int? step_num { get; set; }
        public int? status { get; set; }
    }

}
