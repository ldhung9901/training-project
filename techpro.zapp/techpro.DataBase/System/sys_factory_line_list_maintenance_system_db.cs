using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace techpro.DataBase.System
{
    [Table("sys_factory_line_list_maintenance_system")]
    public class sys_factory_line_list_maintenance_system_db
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public long id { get; set; }
        [MaxLength(500)]
        public string id_factory { get; set; }
        [MaxLength(500)]
        public string id_factory_line { get; set; }
        [MaxLength(500)]
        public string id_system { get; set; }
        [MaxLength(500)]
        public string note { get; set; }
        [MaxLength(128)]
        public string create_by { get; set; }
        [Column(TypeName="DateTime")]
        public DateTime? create_date { get; set; }


    }
}
