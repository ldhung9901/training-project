using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Text;
using techpro.DataBase.System;

namespace techpro.system.data.Models
{
    public class sys_item_model
    {
        public sys_item_model()
        {
            db = new sys_item_db();
        }
        public string createby_name { get; set; }
        public string sys_item_type_name { get; set; }
        public string sys_unit_name { get; set; }
        public string sys_cost_type_name { get; set; }
        public string sys_cost_type_code { get; set; }
        public decimal? inventory_quantity { get; set; }
        public sys_item_db db { get; set; }
        public List<sys_item_quality_model> list_item_quality { get; set; }

        public List<sys_item_unit_other_model> list_sys_item_unit_other { get; set; }
        public List<sys_item_min_max_stock_model> list_item_min_max_stock { get; set; }
    }
    public class sys_item_unit_other_model
    {
        public sys_item_unit_other_model()
        {
            db = new sys_item_unit_other_db();

        }

        public string sys_unit_name { get; set; }
        public sys_item_unit_other_db db { get; set; }
    }
    public class sys_item_min_max_stock_model
    {
        public sys_item_min_max_stock_model()
        {
            db = new sys_item_min_max_stock_db();
        }

        public sys_item_min_max_stock_db db { get; set; }
    }
    public class sys_item_quality_model
    {
        public sys_item_quality_model()
        {
            db = new sys_item_quality_db();
        }
        public List<sys_item_quality_detail_db> list_item_quality { get; set; }
        public sys_item_quality_db db { get; set; }

    }
    public class sys_item_quality_detail
    {
        public sys_item_quality_detail()
        {
            db = new sys_item_quality_detail_db();
        }
        public sys_item_quality_detail_db db { get; set; }

    }
}
