using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Text;
using techpro.DataBase.System;

namespace techpro.system.data.Models
{
    public class sys_factory_model
    {
        public sys_factory_model()
        {
            db = new sys_factory_db();
        }

        public string createby_name { get; set; }
        public sys_factory_db db { get; set; }
    }
}
