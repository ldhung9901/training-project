using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace techpro.DataBase.System
{
    [Table("sys_job_title")]
    public class sys_job_title_db
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [MaxLength(128)]
        public string id { get; set; }
        [MaxLength(500)]
        public string name { get; set; }
        [MaxLength(500)]
        public string note { get; set; }
        [MaxLength(128)]
        public string job_title_code { get; set; }
        [MaxLength(128)]
        public string create_by { get; set; }
        [Column(TypeName="DateTime")]
        public DateTime? create_date { get; set; }
        [MaxLength(128)]
        public string update_by { get; set; }
        [Column(TypeName="DateTime")]
        public DateTime? update_date { get; set; }
        public int? status_del { get; set; }
    }
   
}
