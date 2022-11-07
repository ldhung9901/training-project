using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace techpro.DataBase.System
{
    [Table("sys_item_unit_other")]
    public class sys_item_unit_other_db
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public long id { get; set; }
        [MaxLength(500)]
        public string id_unit { get; set; }
        [MaxLength(500)]
        public string id_item { get; set; }
        [Column(TypeName = "decimal(18, 4)")]
        public decimal? conversion_factor { get; set; }
        [MaxLength(500)]
        public string note { get; set; }
    }
   
}
