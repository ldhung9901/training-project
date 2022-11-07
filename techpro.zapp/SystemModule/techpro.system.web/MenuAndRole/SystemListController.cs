using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Text;

using techpro.common.Models;
using techpro.system.web.Controller;

namespace techpro.system.web.MenuAndRole
{
    public static class SystemListController
    {
        public static List<ControllerAppModel> listController = new List<ControllerAppModel>()
        {
            sys_departmentController.declare,
            sys_job_titleController.declare,
            sys_group_userController.declare,
            sys_userController.declare,
            sys_approval_configController.declare,
            sys_receiving_typeController.declare,
            sys_delivery_typeController.declare,
            sys_cost_typeController.declare,
            sys_warehouseController.declare,
            sys_inventory_position_item_capacityController.declare,
            sys_factoryController.declare,
            sys_factory_lineController.declare,
            sys_factory_line_item_capacityController.declare,
            // sys_phaseController.declare,
            sys_vendor_itemController.declare,
            //  sys_work_schedule_factory_lineController.declare,
            sys_unitController.declare,
            sys_item_typeController.declare,
            sys_itemController.declare,
            sys_item_specificationController.declare,
            sys_item_bomController.declare,
            sys_customerController.declare,
            // sys_opc_clientController.declare,
            // sys_opcController.declare,
            // sys_template_opcController.declare,
            // sys_template_opc_paramController.declare,
            sys_workstationController.declare,
            // sys_workstation_template_opcController.declare,
            // sys_opc_ViewController.declare,
            sys_help_detailController.declare,
            sys_helpController.declare,
            sys_help_pageController.declare,
            sys_versionController.declare,
            sys_version_historyController.declare,
            sys_mongodb_user_auth_logController.declare,
            sys_configurationController.declare,
            sys_phaseController.declare
        };


    }
}
