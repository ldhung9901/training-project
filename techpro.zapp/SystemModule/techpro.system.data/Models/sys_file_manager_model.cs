using System;
using System.Collections.Generic;
using System.Text;
using techpro.DataBase.System;

namespace techpro.system.data.Models
{
    public class sys_file_manager_model
    {
        public sys_file_manager_model()
        {
            db = new sys_file_manager_db();
        }

        public string createby_name { get; set; }
        public sys_file_manager_db db { get; set; }
    }
}
