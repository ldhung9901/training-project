using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Text;
using techpro.DataBase.System;

namespace techpro.system.data.Models
{
    public class sys_opc_model
    {
        public sys_opc_model()
        {
            db = new sys_opc_db();
        }

        public string createby_name { get; set; }

        public string unit_name { get; set; }

        public string opcclient_name { get; set; }
        
        public sys_opc_db db { get; set; }
    }
}
