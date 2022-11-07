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
    partial class sys_unitController
    {
        public static ControllerAppModel declare = new ControllerAppModel()
        {
            controller = "sys_unit",
            // icon = "menu",
            module = "system",
            id = "sys_unit",
            url = "/sys_unit_index",
            title = "sys_unit",
            translate = "NAV.sys_unit",
            type = "item",
            list_controller_action_public = new List<string>(){
                "sys_unit;getListUse",
                "sys_unit;getListUseOtherUnit",
            },
            list_role = new List<ControllerRoleModel>()
            {
                 new ControllerRoleModel()
                {
                    id="sys_unit;create",
                    name="create",
                    list_controller_action = new List<string>()
                    {
                          "sys_unit;create",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_unit;edit",
                    name="edit",
                    list_controller_action = new List<string>()
                    {
                          "sys_unit;edit",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_unit;delete",
                    name="delete",
                    list_controller_action = new List<string>()
                    {
                          "sys_unit;delete",
                    }
                },
                  new ControllerRoleModel()
                {
                    id="sys_unit;list",
                    name="list",
                    list_controller_action = new List<string>()
                    {
                          "sys_unit;DataHandler",
                    }
                }
            }
        };
        private bool checkModelStateCreate(sys_unit_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.create, item);
        }

        private bool checkModelStateEdit(sys_unit_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.edit, item);
        }
        private bool checkModelStateCreateEdit(ActionEnumForm action, sys_unit_model item)
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
