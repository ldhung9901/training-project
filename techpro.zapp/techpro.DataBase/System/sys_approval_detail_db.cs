using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace techpro.DataBase.System
{
    [Table("sys_approval_detail")]
    public class sys_approval_detail_db
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public long id { get; set; }
        public int? status_finish { get; set; }
        [MaxLength(500)]
        public string to_user { get; set; }
        [MaxLength(500)]
        public string from_user { get; set; }
        public int? step_num { get; set; }
        [MaxLength(500)]
        public string step_name { get; set; }
        [MaxLength(500)]
        public string id_approval_config { get; set; }
        [MaxLength(500)]
        public string note { get; set; }
        [Column(TypeName="DateTime")]
        public DateTime? date_action { get; set; }
        [MaxLength(500)]
        public string user_action { get; set; }
        [Column(TypeName="DateTime")]
        public DateTime? deadline { get; set; }
        [MaxLength(500)]
        public string id_approval { get; set; }
        public int? status_action { get; set; }

    }
}
