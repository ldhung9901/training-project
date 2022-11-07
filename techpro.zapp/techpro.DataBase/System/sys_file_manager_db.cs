using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace techpro.DataBase.System
{
    [Table("sys_file_manager")]
    public class sys_file_manager_db
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        [MaxLength(128)]
        public string id_record { get; set; }
        [MaxLength(500)]
        public string file_name { get; set; }
        [MaxLength(500)]
        public string menu { get; set; }
        [MaxLength(50)]
        public string file_type { get; set; }
        [MaxLength(100)]
        public string file_size { get; set; }
        [MaxLength(500)]
        public string file_path { get; set; }
        [MaxLength(200)]
        public string color { get; set; }

        public int? status_del { get; set; }
        [MaxLength(128)]
        public string create_by { get; set; }
        [Column(TypeName = "DateTime")]
        public DateTime? create_date { get; set; }

    }

}
