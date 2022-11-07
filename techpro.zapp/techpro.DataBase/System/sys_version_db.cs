using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace techpro.DataBase.System
{
    [Table("sys_version")]
    public class sys_version_db
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [MaxLength(128)]
        public string id { get; set; }
        [MaxLength(500)]
        public string version { get; set; }
        [Column(TypeName = "DateTime")]
        public DateTime? release_day { get; set; }
        [Column(TypeName = "nvarchar(MAX)")]
        public string note { get; set; }
        [MaxLength(500)]
        public string create_by { get; set; }
        [Column(TypeName = "DateTime")]
        public DateTime? create_date { get; set; }
        [MaxLength(500)]
        public string update_by { get; set; }
        [Column(TypeName = "DateTime")]
        public DateTime? update_date { get; set; }
        [MaxLength(128)]
        public int? status_del { get; set; }
    }

}
