using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace techpro.DataBase.System
{
    [Table("sys_approval")]
    public class sys_approval_db
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [MaxLength(128)]
        public string id { get; set; }
        [MaxLength(128)]
        public string create_by { get; set; }
        [Column(TypeName="DateTime")]
        public DateTime? create_date { get; set; }
        [MaxLength(500)]
        public string create_by_record { get; set; }
        [Column(TypeName="DateTime")]
        public DateTime? create_date_record { get; set; }
        [MaxLength(500)]
        public string start_by { get; set; }
        [Column(TypeName="DateTime")]
        public DateTime? start_date { get; set; }

        [MaxLength(500)]
        public string id_sys_approval_config { get; set; }
        // 1 new, 2 in process, 3 finish, 4 cancel,5 return, 6 close
        public int? status_finish { get; set; }
        [MaxLength(500)]
        public string to_user { get; set; }
        [MaxLength(500)]
        public string from_user { get; set; }
        public int? step_num { get; set; }
        [MaxLength(500)]
        public string last_note { get; set; }
        [Column(TypeName="DateTime")]
        public DateTime? last_date_action { get; set; }
        [MaxLength(500)]
        public string last_user_action { get; set; }
        public int? total_step { get; set; }
        [MaxLength(500)]
        public string menu { get; set; }
        [MaxLength(500)]
        public string id_record { get; set; }
        [Column(TypeName="DateTime")]
        public DateTime? deadline { get; set; }
        public int? status_action { get; set; }
        [MaxLength(500)]
        public string step_name { get; set; }
    }
}
