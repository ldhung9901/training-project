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
    partial class sys_workstationController
    {
        public static ControllerAppModel declare = new ControllerAppModel()
        {
            controller = "sys_workstation",
            // icon = "developer_board",
            module = "system",
            id = "sys_workstation",
            url = "/sys_workstation_index",
            title = "sys_workstation",
            translate = "NAV.sys_workstation",
            type = "item",
            list_controller_action_public = new List<string>(){
                "sys_workstation;getopcconfig",
                 "sys_workstation;getopcvalue",
                          "sys_workstation;getListUse",

            },
            list_role = new List<ControllerRoleModel>()
            {
                 new ControllerRoleModel()
                {
                    id="sys_workstation;create",
                    name="create",
                    list_controller_action = new List<string>()
                    {
                          "sys_workstation;create",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_workstation;edit",
                    name="edit",
                    list_controller_action = new List<string>()
                    {
                          "sys_workstation;edit",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_workstation;delete",
                    name="delete",
                    list_controller_action = new List<string>()
                    {
                          "sys_workstation;delete",
                    }
                },
                  new ControllerRoleModel()
                {
                    id="sys_workstation;list",
                    name="list",
                    list_controller_action = new List<string>()
                    {
                          "sys_workstation;DataHandler",
                         
                          
                    }
                }
            }
        };
        private bool checkModelStateCreate(sys_workstation_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.create, item);
        }

        private bool checkModelStateEdit(sys_workstation_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.edit, item);
        }
        private bool checkModelStateCreateEdit(ActionEnumForm action, sys_workstation_model item)
        {
            if (string.IsNullOrWhiteSpace(item.db.name))
            {
                ModelState.AddModelError("db.name", "required");
            }
            // if (string.IsNullOrWhiteSpace(item.db.id_sys_factory))
            // {
            //     ModelState.AddModelError("db.id_sys_factory", "required");
            // }
            // if (string.IsNullOrWhiteSpace(item.db.id_sys_factory_line))
            // {
            //     ModelState.AddModelError("db.id_sys_factory_line", "required");
            // }
            var search = repo.FindAll().Where(d => d.db.name == item.db.name && d.db.id != item.db.id).Count();
            if (search > 0)
            {
                ModelState.AddModelError("db.name", "existed");
            }
             var search1 = repo.FindAll().Where(d => d.db.id_sys_phase == item.db.id_sys_phase && d.db.id != item.db.id && d.db.id_sys_factory == item.db.id_sys_factory && d.db.id_sys_factory_line == item.db.id_sys_factory_line).Count();
            if (search1 > 0)
            {
                ModelState.AddModelError("db.id_sys_phase", "existed");
            }
          
            return ModelState.IsValid;
        }

    }
}
