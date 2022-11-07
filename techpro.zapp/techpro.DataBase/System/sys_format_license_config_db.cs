using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace techpro.DataBase.System
{
    [Table("sys_format_license_config")]
    public class sys_format_license_config_db
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public long id { get; set; }
        [MaxLength(500)]
        public string first_character { get; set; }
        public bool? is_raise { get; set; }
        [MaxLength(500)]
        public string menu { get; set; }        
        [MaxLength(128)]
        public string receiptCode { get; set; }        
        [Column(TypeName="DateTime")]
        public DateTime? create_date { get; set; }
        [MaxLength(128)]
        public string create_by { get; set; }
        [MaxLength(128)]
        public string update_by { get; set; }
        public int? status_del { get; set; }
        [Column(TypeName="DateTime")]
        public DateTime? update_date { get; set; }

    }
}
