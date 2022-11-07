using System;
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
    partial class sys_help_pageController
    {
        public static ControllerAppModel declare = new ControllerAppModel()
        {
            controller = "sys_help_page",
            // icon = "live_help",
            module = "system",
            id = "sys_help_page",
            url = "/sys_help_page_index",
            title = "sys_help_page",
            translate = "NAV.sys_help_page",
            type = "item",
            list_controller_action_public = new List<string>(){
                "sys_help_page;getListUse",
            },
            list_role = new List<ControllerRoleModel>()
            {

                  new ControllerRoleModel()
                {
                    id="sys_help_page;list",
                    name="list",
                    list_controller_action = new List<string>()
                    {
                          "sys_help_page;DataHandler",
                    }
                }
            }
        };
      

    }
}
