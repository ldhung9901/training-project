using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace techpro.DataBase.System
{
    [Table("sys_customer")]
    public class sys_customer_db
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [MaxLength(128)]
        public string id { get; set; }
        [MaxLength(500)]
        public string name { get; set; }
        [MaxLength(128)]
        public string customer_code { get; set; }
        [MaxLength(500)]
        public string address { get; set; }
        [MaxLength(500)]
        public string email { get; set; }
        [MaxLength(500)]
        public string phone { get; set; }
        [MaxLength(500)]
        public string fax { get; set; }
        [MaxLength(500)]
        public string customer_service { get; set; }
        [MaxLength(500)]
        public string name_short { get; set; }
        [MaxLength(500)]
        public string logo_path { get; set; }
        [MaxLength(500)]
        public string tax_number { get; set; }
        [MaxLength(500)]
        public string note { get; set; }
        public bool? is_customer { get; set; }
        public bool? is_supplier { get; set; }
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
