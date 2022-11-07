using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Text;
using techpro.DataBase.System;

namespace techpro.system.data.Models
{
    public class sys_configuration_model
    {
         public sys_configuration_model()
        {
            db_company = new sys_company_db();
        }
        public string createby_name { get; set; }
        public sys_company_db db_company { get; set; }
       
        
    }

     public class sys_configuration_model_v2
    {
         public sys_configuration_model_v2()
        {
            db_format_license_config = new List<sys_format_license_config_db>();
        }
        public string createby_name { get; set; }
        public List<sys_format_license_config_db> db_format_license_config { get; set; }
        
    }

    public class sys_working_time_model
    {
         public sys_working_time_model()
        {
            db = new sys_working_time_config_db();
            list_schedule = new List<sys_working_time_detail_model>();
        }
        public string createby_name { get; set; }
        public List<sys_working_time_detail_model> list_schedule { get; set; }
        public sys_working_time_config_db db { get; set; }
        
    }

    public class sys_working_time_detail_model
    {
         public sys_working_time_detail_model()
        {
            db = new sys_working_time_detail_config_db();

        }
        public string createby_name { get; set; }
        public sys_working_time_detail_config_db db { get; set; }
        
    }
}
