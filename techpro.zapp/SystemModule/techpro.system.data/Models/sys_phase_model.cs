using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Text;
using techpro.DataBase.System;

namespace techpro.system.data.Models
{
    public class sys_phase_model
    {
        public sys_phase_model()
        {
            db = new sys_phase_db();
        }

        public string createby_name { get; set; }
        public sys_phase_db db { get; set; }
    }
}
