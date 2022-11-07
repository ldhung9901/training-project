﻿using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace techpro.DataBase.System
{
    [Table("sys_group_user_detail")]
    public class sys_group_user_detail_db
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public long id { get; set; }
        [MaxLength(500)]
        public string id_group_user { get; set; }
        [MaxLength(500)]
        public string user_id { get; set; }
    }
}
