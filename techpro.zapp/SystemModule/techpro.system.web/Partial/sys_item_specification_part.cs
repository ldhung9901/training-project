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
    partial class sys_item_specificationController
    {
        public static ControllerAppModel declare = new ControllerAppModel()
        {
            controller = "sys_item_specification",
            // icon = "menu",
            module = "system",
            id = "sys_item_specification",
            url = "/sys_item_specification_index",
            title = "sys_item_specification",
            translate = "NAV.sys_item_specification",
            type = "item",
            list_controller_action_public = new List<string>(){
                "sys_item_specification;getListUse",
                "sys_item_specification;get_item_specification"
            },
            list_role = new List<ControllerRoleModel>()
            {
                 new ControllerRoleModel()
                {
                    id="sys_item_specification;create",
                    name="create",
                    list_controller_action = new List<string>()
                    {
                          "sys_item_specification;create",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_item_specification;edit",
                    name="edit",
                    list_controller_action = new List<string>()
                    {
                          "sys_item_specification;edit",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_item_specification;delete",
                    name="delete",
                    list_controller_action = new List<string>()
                    {
                          "sys_item_specification;delete",
                    }
                },
                  new ControllerRoleModel()
                {
                    id="sys_item_specification;list",
                    name="list",
                    list_controller_action = new List<string>()
                    {
                          "sys_item_specification;DataHandler",
                    }
                }
            }
        };
        private bool checkModelStateCreate(sys_item_specification_model item)
        {
            if (string.IsNullOrWhiteSpace(item.db.id_item))
            {
                ModelState.AddModelError("db.id_item", "required");
            }
            return checkModelStateCreateEdit(ActionEnumForm.create, item);
        }

        private bool checkModelStateEdit(sys_item_specification_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.edit, item);
        }
        private bool checkModelStateCreateEdit(ActionEnumForm action, sys_item_specification_model item)
        {
            if (string.IsNullOrWhiteSpace(item.db.name))
            {
                ModelState.AddModelError("db.name", "required");
            }
            if (string.IsNullOrWhiteSpace(item.db.id_unit))
            {
                ModelState.AddModelError("db.id_unit", "required");
            }
            if ((item.db.conversion_factor??0)<=0)
            {
                ModelState.AddModelError("db.conversion_factor", "required");
            }
            var search = repo.FindAll().Where(d =>
            d.db.id_item ==  item.db.id_item &&
            d.db.name == item.db.name && d.db.id != item.db.id).Count();
            if (search > 0)
            {
                ModelState.AddModelError("db.name", "existed");
            }


            return ModelState.IsValid;
        }

    }
}
