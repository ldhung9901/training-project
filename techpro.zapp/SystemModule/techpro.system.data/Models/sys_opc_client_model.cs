using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Text;
using techpro.DataBase.System;

namespace techpro.system.data.Models
{
    public class sys_opc_client_model
    {
        public sys_opc_client_model()
        {
            db = new sys_opc_client_db();
        }

        public string createby_name { get; set; }
        public sys_opc_client_db db { get; set; }
     
    }
}
