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
    partial class sys_cost_typeController
    {
        public static ControllerAppModel declare = new ControllerAppModel()
        {
            controller = "sys_cost_type",
            // icon = "menu",
            module = "system",
            id = "sys_cost_type",
            url = "/sys_cost_type_index",
            title = "sys_cost_type",
            translate = "NAV.sys_cost_type",
            type = "item",
            list_controller_action_public = new List<string>(){
                "sys_cost_type;getListUse",
            },
            list_role = new List<ControllerRoleModel>()
            {
                 new ControllerRoleModel()
                {
                    id="sys_cost_type;create",
                    name="create",
                    list_controller_action = new List<string>()
                    {
                          "sys_cost_type;create",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_cost_type;edit",
                    name="edit",
                    list_controller_action = new List<string>()
                    {
                          "sys_cost_type;edit",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_cost_type;delete",
                    name="delete",
                    list_controller_action = new List<string>()
                    {
                          "sys_cost_type;delete",
                    }
                },
                  new ControllerRoleModel()
                {
                    id="sys_cost_type;list",
                    name="list",
                    list_controller_action = new List<string>()
                    {
                          "sys_cost_type;DataHandler",
                    }
                }
            }
        };
        private bool checkModelStateCreate(sys_cost_type_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.create, item);
        }

        private bool checkModelStateEdit(sys_cost_type_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.edit, item);
        }
        private bool checkModelStateCreateEdit(ActionEnumForm action, sys_cost_type_model item)
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

            if (string.IsNullOrWhiteSpace(item.db.cost_type_code))
            {
                ModelState.AddModelError("db.cost_type_code", "required");
            }
            var search_cost_type_code = repo.FindAll().Where(d => d.db.cost_type_code == item.db.cost_type_code && d.db.id != item.db.id).Count();
            if (search_cost_type_code > 0)
            {
                ModelState.AddModelError("db.cost_type_code", "existed");
            }
            return ModelState.IsValid;
        }

    }
}
