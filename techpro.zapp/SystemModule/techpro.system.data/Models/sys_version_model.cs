using System;
using System.Collections.Generic;
using System.Text;
using techpro.DataBase.System;

namespace techpro.system.data.Models
{
    public class sys_version_model
    {
        public sys_version_model()
        {
            db = new sys_version_db();
        }
        public string createby_name { get; set; }
        public sys_version_db db { get; set; }
    }
}
