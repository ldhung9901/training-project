using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace techpro.DataBase.System
{
    [Table("sys_workstation")]
    public class sys_workstation_db
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [MaxLength(128)]
        public string id { get; set; }
  
        [MaxLength(500)]
        public string name { get; set; }
        [MaxLength(500)]
        public string note { get; set; }
        [MaxLength(500)]
        public string ip_address { get; set; }
        [MaxLength(500)]
        public string id_worker { get; set; }
        [MaxLength(500)]
        public string id_sys_factory { get; set; }
        [MaxLength(500)]
        public string id_sys_phase { get; set; }
        [MaxLength(500)]
        public string manage_workstation { get; set; }
        [MaxLength(500)]
        public string pathimg { get; set; }
        [MaxLength(500)]
        public string id_sys_factory_line { get; set; }
        [MaxLength(128)]
        public string update_by { get; set; }
        [Column(TypeName="DateTime")]
        public DateTime? update_date { get; set; }
        [Column(TypeName="DateTime")]
        public DateTime? create_date { get; set; }
        [MaxLength(128)]
        public string create_by { get; set; }
        public int? status_del { get; set; }

    }
}
