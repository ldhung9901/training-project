using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace techpro.DataBase.System
{
    [Table("users")]
    public class User
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [MaxLength(128)]
        public string Id { get; set; }
        [MaxLength(500)]
        public string avatar_path { get; set; }
        [MaxLength(500)]
        public string FirstName { get; set; }
        [MaxLength(500)]
        public string LastName { get; set; }
        [MaxLength(500)]
        public string Username { get; set; }
        [Column(TypeName="DateTime")]
        public DateTime? start_work_date { get; set; }
        [MaxLength(500)]
        public string phone_number { get; set; }
        [MaxLength(500)]
        public string email { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        [MaxLength(500)]
        public string id_department { get; set; }
        [MaxLength(500)]
        public string id_job_title { get; set; }
    }
}