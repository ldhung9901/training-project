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
    partial class sys_customerController
    {
        public static ControllerAppModel declare = new ControllerAppModel()
        {
            controller = "sys_customer",
            // icon = "contact_mail",
            module = "system",
            id = "sys_customer",
            url = "/sys_customer_index",
            title = "sys_customer",
            translate = "NAV.sys_customer",
            type = "item",
            list_controller_action_public = new List<string>(){
                "sys_customer;getListUse",
                "sys_customer;getListUseSupplier",
                "sys_customer;getListUseCustomner",
                "sys_customer;getElementById",
            },
            list_role = new List<ControllerRoleModel>()
            {
                 new ControllerRoleModel()
                {
                    id="sys_customer;create",
                    name="create",
                    list_controller_action = new List<string>()
                    {
                          "sys_customer;create",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_customer;edit",
                    name="edit",
                    list_controller_action = new List<string>()
                    {
                          "sys_customer;edit",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_customer;delete",
                    name="delete",
                    list_controller_action = new List<string>()
                    {
                          "sys_customer;delete",
                    }
                },
                  new ControllerRoleModel()
                {
                    id="sys_customer;list",
                    name="list",
                    list_controller_action = new List<string>()
                    {
                          "sys_customer;DataHandler",
                    }
                }
            }
        };

        private bool checkModelStateCreate(sys_customer_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.create, item);
        }

        private bool checkModelStateEdit(sys_customer_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.edit, item);
        }
        private bool checkModelStateCreateEdit(ActionEnumForm action, sys_customer_model item)
        {
            if (string.IsNullOrWhiteSpace(item.db.name))
            {
                ModelState.AddModelError("db.name", "required");
            }
            if (string.IsNullOrWhiteSpace(item.db.tax_number))
            {
                ModelState.AddModelError("db.tax_number", "required");
            }
            var search = repo.FindAll().Where(d => d.db.name == item.db.name && d.db.id != item.db.id).Count();
            if (search > 0)
            {
                ModelState.AddModelError("db.name", "existed");
            }
            var searchTax = repo.FindAll().Where(d => d.db.name == item.db.name && d.db.id != item.db.id).Where(d => d.db.tax_number == item.db.tax_number && d.db.id != item.db.id).Count();
            if (search > 0)
            {
                ModelState.AddModelError("db.tax_number", "existed");
            }

            return ModelState.IsValid;
        }

    }
}
