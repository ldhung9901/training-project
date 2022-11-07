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
    partial class sys_itemController
    {
        public static ControllerAppModel declare = new ControllerAppModel()
        {
            controller = "sys_item",
            // icon = "list_alt",
            module = "system",
            id = "sys_item",
            url = "/sys_item_index",
            title = "sys_item",
            translate = "NAV.sys_item",
            type = "item",
            list_controller_action_public = new List<string>(){
                "sys_item;getListUse",
                "sys_item;getListUnitOther",
                "sys_item;getListMinMaxStock",
                "sys_item;getListUseOtherUnit",
                "sys_item;getItemByBarcode",
                "sys_item;getListQuanlity",
                "sys_item;get_item_exist"
            },
            list_role = new List<ControllerRoleModel>()
            {
                 new ControllerRoleModel()
                {
                    id="sys_item;create",
                    name="create",
                    list_controller_action = new List<string>()
                    {
                          "sys_item;create",
                          "sys_item;create_item",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_item;edit",
                    name="edit",
                    list_controller_action = new List<string>()
                    {
                          "sys_item;edit",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_item;delete",
                    name="delete",
                    list_controller_action = new List<string>()
                    {
                          "sys_item;delete",
                          "sys_item;delete_item",
                    }
                },
                  new ControllerRoleModel()
                {
                    id="sys_item;list",
                    name="list",
                    list_controller_action = new List<string>()
                    {
                          "sys_item;DataHandler",
                    }
                }
            }
        };
        private bool checkModelStateCreate(sys_item_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.create, item);
        }

        private bool checkModelStateEdit(sys_item_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.edit, item);
        }
        private bool checkModelStateCreateEdit(ActionEnumForm action, sys_item_model item)
        {

            if (string.IsNullOrWhiteSpace(item.db.name))
            {
                ModelState.AddModelError("db.name", "required");
            }

            var search_name = repo.FindAll().Where(d => d.db.name == item.db.name && d.db.id != item.db.id).Count();
            if (search_name > 0)
            {
                ModelState.AddModelError("db.name", "existed");
            }

            if (string.IsNullOrWhiteSpace(item.db.code_item))
            {
                ModelState.AddModelError("db.code_item", "required");
            }
            var search_code_item = repo.FindAll().Where(d => d.db.code_item == item.db.code_item && d.db.id != item.db.id).Count();
            if (search_code_item > 0)
            {
                ModelState.AddModelError("db.code_item", "existed");
            }

            if (item.db.type==null)
            {
                ModelState.AddModelError("db.type", "required");
            }
            if (item.list_item_quality.Count == 0)
            {
                ModelState.AddModelError("list_item_quality", "required");
            }
            if (string.IsNullOrWhiteSpace(item.db.id_unit))
            {
                ModelState.AddModelError("db.id_unit", "required");
            }
           
            return ModelState.IsValid;
        }

    }
}
