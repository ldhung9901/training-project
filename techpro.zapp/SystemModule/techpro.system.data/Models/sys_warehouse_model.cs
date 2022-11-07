using techpro.DataBase.System;

namespace techpro.system.data.Models
{
    public class sys_warehouse_model
    {
        public sys_warehouse_model()
        {
            db = new sys_warehouse_db();
        }

        public string createby_name { get; set; }
        public sys_warehouse_db db { get; set; }
    }
}
