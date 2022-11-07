using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace techpro.DataBase.System
{
    [Table("sys_work_schedule_factory_line")]
     public class sys_work_schedule_factory_line_db
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [MaxLength(128)]
        public long id { get; set; }
        [MaxLength(500)]
        public string id_sys_factory { get; set; }
        [MaxLength(500)]
        public string id_sys_factory_line { get; set; }
        [Column(TypeName = "decimal(18, 4)")]
        public decimal? daysOfWork { get; set; }
        [Column(TypeName="DateTime")]
        public DateTime? timeStart_1 { get; set; }
        [Column(TypeName="DateTime")]
        public DateTime? timeEnd_1 { get; set; }
        [Column(TypeName="DateTime")]
        public DateTime? timeStart_2 { get; set; }
        [Column(TypeName="DateTime")]
        public DateTime? timeEnd_2 { get; set; }
        [Column(TypeName="DateTime")]
        public DateTime? timeStart_3 { get; set; }
        [Column(TypeName="DateTime")]
        public DateTime? timeEnd_3 { get; set; }
        [MaxLength(128)]
        public string create_by { get; set; }
        [Column(TypeName="DateTime")]
        public DateTime? create_date { get; set; }
        [Column(TypeName="DateTime")]
        public DateTime? update_date { get; set; }
        [MaxLength(128)]
        public string update_by { get; set; }
        [MaxLength(500)]
        public string note { get; set; }

      
    }
}
