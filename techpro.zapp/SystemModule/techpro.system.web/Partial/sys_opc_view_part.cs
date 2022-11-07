using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using techpro.common.BaseClass;
using techpro.common.Models;
using techpro.system.data.Models;

namespace techpro.system.web.Controller
{
    partial class sys_opc_ViewController
    {
        public static ControllerAppModel declare = new ControllerAppModel()
        {
            controller = "sys_opc_view",
            icon = "menu",
            module = "system",
            id = "sys_opc_view",
            url = "/sys_opc_view_index",
            title = "sys_opc_view",
            translate = "NAV.sys_opc_view",
            type = "item",
            list_controller_action_public = new List<string>(){

                       "sys_opc_view;getProductionOrder",
            },
            list_role = new List<ControllerRoleModel>()
            {
               
                  new ControllerRoleModel()
                {
                    id="sys_opc_view;list",
                    name="list",
                    list_controller_action = new List<string>()
                    {
                          "sys_opc_view;DataHandler",
                         
                          
                    }
                }
            }
        };
     

    }
}
