using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Text;
using techpro.DataBase.System;

namespace techpro.system.data.Models
{
    public class sys_template_opc_model
    {
        public sys_template_opc_model()
        {
            db = new sys_template_opc_db();
        }

        public string createby_name { get; set; }

        public string opcnode_error_name { get; set; }

        public string opcnode_input_name { get; set; }

        public string opcnode_output_name { get; set; }
        public sys_template_opc_db db { get; set; }
     
    }
}
