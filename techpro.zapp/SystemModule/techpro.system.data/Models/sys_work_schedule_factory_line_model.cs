using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Text;
using techpro.DataBase.System;

namespace techpro.system.data.Models
{
   public class sys_work_schedule_factory_line_model
    {
        public sys_work_schedule_factory_line_model()
        {
            db = new sys_work_schedule_factory_line_db();
        }

        public string updateby_name { get; set; }

        public string  createby_name { get; set; }


        public string factory_name { get; set; }

        public string factory_line_name { get; set; }
        


        public string day_name { get; set; }
        public sys_work_schedule_factory_line_db db { get; set; }

    }
}
