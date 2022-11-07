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
    partial class sys_departmentController
    {
        public static ControllerAppModel declare = new ControllerAppModel()
        {
            controller = "sys_department",
            // icon = "business",
            module = "system",
            id = "sys_department",
            url = "/sys_department_index",
            title = "sys_department",
            translate = "NAV.sys_department",
            type = "item",
            list_controller_action_public = new List<string>(){
                "sys_department;getListUse",
            },
            list_role = new List<ControllerRoleModel>()
            {
                 new ControllerRoleModel()
                {
                    id="sys_department;create",
                    name="create",
                    list_controller_action = new List<string>()
                    {
                          "sys_department;create",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_department;edit",
                    name="edit",
                    list_controller_action = new List<string>()
                    {
                          "sys_department;edit",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_department;delete",
                    name="delete",
                    list_controller_action = new List<string>()
                    {
                          "sys_department;delete",
                    }
                },
                  new ControllerRoleModel()
                {
                    id="sys_department;list",
                    name="list",
                    list_controller_action = new List<string>()
                    {
                          "sys_department;DataHandler",
                    }
                }
            }
        };

        private bool checkModelStateCreate(sys_department_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.create, item);
        }

        private bool checkModelStateEdit(sys_department_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.edit, item);
        }
        private bool checkModelStateCreateEdit(ActionEnumForm action, sys_department_model item)
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
            if (string.IsNullOrWhiteSpace(item.db.department_code))
            {
                ModelState.AddModelError("db.department_code", "required");
            }
            var search_code = repo.FindAll().Where(d => d.db.department_code == item.db.department_code && d.db.id != item.db.id).Count();
            if (search_code > 0)
            {
                ModelState.AddModelError("db.department_code", "existed");
            }


            return ModelState.IsValid;
        }

    }
}
