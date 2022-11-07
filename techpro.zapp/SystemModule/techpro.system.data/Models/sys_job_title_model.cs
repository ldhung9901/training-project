using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Text;
using techpro.DataBase.System;


namespace techpro.system.data.Models
{
    public class sys_job_title_model
    {
        public sys_job_title_model()
        {
            db = new sys_job_title_db();
        }

        public string createby_name { get; set; }
        public sys_job_title_db db { get; set; }
    }
}
