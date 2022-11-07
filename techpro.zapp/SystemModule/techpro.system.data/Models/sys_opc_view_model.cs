using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Text;
using techpro.DataBase.System;

namespace techpro.system.data.Models
{
    public class sys_opc_view_model
    {

        public string opc_client { get; set; }

        public string opc_node_input { get; set; }

        public string opc_node_output { get; set; }

        public string opc_node_error { get; set; }

        public string template_opc_name { get; set; }

        public string value_input { get; set; }

        public string value_output { get; set; }

        public string value_error { get; set; }

        public List<sys_opc_view_detail> list_param { get; set; }
    }
    public class sys_opc_view_detail
    {

        public string opc_node { get; set; }

        public string value { get; set; }

        public string name { get; set; }

        public string unit_name { get; set; }
    }

    public class sys_opc_view_production_order
    {

        public string id_production_order { get; set; }

        public string id_item { get; set; }

        public string production_order_number { get; set; }

        public string item_name { get; set; }

        public string item_specification_name { get; set; }

        public string item_full_name { get { return item_name + (item_specification_name ?? ""); } }

        public string unit_main_name { get; set; }

        public string unit_name { get; set; }

        public string template_opc_name { get; set; }
        public decimal? quantity_unit_main { get; set; }
        public decimal? quantity { get; set; }
        public int? type_add { get; set; }

        //1 waitting ,2 run, 3 end
        public int? run_status { get; set; }
    }
}
