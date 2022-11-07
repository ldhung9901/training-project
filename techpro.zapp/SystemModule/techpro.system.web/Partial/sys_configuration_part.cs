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
    partial class sys_configurationController
    {
        public static ControllerAppModel declare = new ControllerAppModel()
        {
            controller = "sys_configuration",
            icon = "heroicons_outline:adjustments",
            module = "system",
            id = "sys_configuration",
            url = "/sys_configuration_index",
            title = "sys_configuration",
            translate = "NAV.sys_configuration",
            type = "item",
            list_controller_action_public = new List<string>(){
                "sys_configuration;getListUse",
                "sys_configuration;getElementById",
                "sys_configuration;get_List_license",
                "sys_configuration;get_list_working_time",
                "sys_configuration;getListItem",
            },
            list_role = new List<ControllerRoleModel>()
            {
                new ControllerRoleModel()
                {
                    id="sys_configuration;create",
                    name="create",
                    list_controller_action = new List<string>()
                    {
                          "sys_configuration;create_license",
                          "sys_configuration;create_working_time",
                    }
                },
                 new ControllerRoleModel()
                {
                    id="sys_configuration;list",
                    name="list",
                    list_controller_action = new List<string>()
                    {
                          "sys_configuration;DataHandler",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_configuration;edit",
                    name="edit",
                    list_controller_action = new List<string>()
                    {
                        "sys_configuration;edit_company",
                        "sys_configuration;edit_license",
                        "sys_configuration;edit_working_time",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_configuration;delete",
                    name="delete",
                    list_controller_action = new List<string>()
                    {
                          "sys_configuration;delete_license",
                          "sys_configuration;delete_working_time",
                    }
                }
            }
            
        };
        private bool checkModelStateCreate(sys_configuration_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.create, item);
        }

        private bool checkModelStateEdit(sys_configuration_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.edit, item);
        }
        private bool checkModelStateCreateEdit(ActionEnumForm action, sys_configuration_model item)
        {
            if (string.IsNullOrWhiteSpace(item.db_company.name))
            {
                ModelState.AddModelError("db_company.name", "required");
            }
            // var search = repo.FindAll().Where(d => d.db_company.name == item.db_company.name).Count();
            // if (search > 0)
            // {
            //     ModelState.AddModelError("db_company.name", "existed");
            // }


            return ModelState.IsValid;
        }

        private bool checkModelStateCreate(sys_working_time_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.create, item);
        }

        private bool checkModelStateEdit(sys_working_time_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.edit, item);
        }
        private bool checkModelStateCreateEdit(ActionEnumForm action, sys_working_time_model item)
        {
            if (string.IsNullOrWhiteSpace(item.db.name))
            {
                ModelState.AddModelError("db.name", "required");
            }
            var search = repo.find_working_time().Where(d => d.db.name == item.db.name && d.db.id != item.db.id).Count();
            if (search > 0)
            {
                ModelState.AddModelError("db.name", "existed");
            }
            if (item.list_schedule.Count==0)
            {
                ModelState.AddModelError("list_schedule", "required");
            }
            return ModelState.IsValid;
        }
        
        
    }
}
