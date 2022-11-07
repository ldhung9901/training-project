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
    partial class sys_phaseController
    {
        public static ControllerAppModel declare = new ControllerAppModel()
        {
            controller = "sys_phase",
            // icon = "menu",
            module = "system",
            id = "sys_phase",
            url = "/sys_phase_index",
            title = "sys_phase",
            translate = "NAV.sys_phase",
            type = "item",
            list_controller_action_public = new List<string>(){
                "sys_phase;getListUse",
            },
            list_role = new List<ControllerRoleModel>()
            {
                 new ControllerRoleModel()
                {
                    id="sys_phase;create",
                    name="create",
                    list_controller_action = new List<string>()
                    {
                          "sys_phase;create",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_phase;edit",
                    name="edit",
                    list_controller_action = new List<string>()
                    {
                          "sys_phase;edit",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_phase;delete",
                    name="delete",
                    list_controller_action = new List<string>()
                    {
                          "sys_phase;delete",
                    }
                },
                  new ControllerRoleModel()
                {
                    id="sys_phase;list",
                    name="list",
                    list_controller_action = new List<string>()
                    {
                          "sys_phase;DataHandler",
                    }
                }
            }
        };
        private bool checkModelStateCreate(sys_phase_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.create, item);
        }

        private bool checkModelStateEdit(sys_phase_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.edit, item);
        }
        private bool checkModelStateCreateEdit(ActionEnumForm action, sys_phase_model item)
        {
            if (string.IsNullOrWhiteSpace(item.db.name))
            {
                ModelState.AddModelError("db.name", "required");
            }
            var search = repo.FindAll().Where(d => d.db.name == item.db.name && d.db.id != item.db.id).Count();
            if (search > 0)
            {
                ModelState.AddModelError("db.name", "existed");
            }


            return ModelState.IsValid;
        }

    }
}
