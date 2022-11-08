import { Route } from '@angular/router';
import { sys_approval_config_indexComponent } from './sys_approval_config/index.component';
import { sys_cost_type_indexComponent } from './sys_cost_type/index.component';
import { sys_customer_indexComponent } from './sys_customer/index.component';
import { sys_delivery_type_indexComponent } from './sys_delivery_type/index.component';
import { sys_department_indexComponent } from './sys_department/index.component';
import { sys_factory_indexComponent } from './sys_factory/index.component';
import { sys_factory_line_indexComponent } from './sys_factory_line/index.component';
import { sys_factory_line_item_capacity_indexComponent } from './sys_factory_line_item_capacity/index.component';
import { sys_group_user_indexComponent } from './sys_group_user/index.component';
import { sys_help_indexComponent } from './sys_help/index.component';
import { sys_help_detail_indexComponent } from './sys_help_detail/index.component';
import { sys_inventory_position_item_capacity_indexComponent } from './sys_inventory_position_item_capacity/index.component';
import { sys_item_indexComponent } from './sys_item/index.component';
import { sys_item_bom_indexComponent } from './sys_item_bom/index.component';
import { sys_item_specification_indexComponent } from './sys_item_specification/index.component';
import { sys_item_type_indexComponent } from './sys_item_type/index.component';
import { sys_job_title_indexComponent } from './sys_job_title/index.component';
import { sys_opc_indexComponent } from './sys_opc/index.component';
import { sys_opc_client_indexComponent } from './sys_opc_client/index.component';
import { sys_opc_viewComponent } from './sys_opc_view/index.component';
import { sys_phase_indexComponent } from './sys_phase/index.component';
import { sys_receiving_type_indexComponent } from './sys_receiving_type/index.component';
import { sys_template_opc_indexComponent } from './sys_template_opc/index.component';
import { sys_template_opc_param_indexComponent } from './sys_template_opc_param/index.component';
import { sys_unit_indexComponent } from './sys_unit/index.component';
import { sys_user_indexComponent } from './sys_user/index.component';
import { sys_vendor_item_indexComponent } from './sys_vendor_item/index.component';
import { sys_workstation_indexComponent } from './sys_workstation/index.component';
import { sys_workstation_template_opc_indexComponent } from './sys_workstation_template_opc/index.component';
import { sys_work_schedule_factory_lineindexComponent } from './sys_work_schedule_factory_line/index.component';
import { sys_help_page_indexComponent } from './sys_help_page/index.component';
import { sys_help_page_detailComponent } from './sys_help_page/detail.component';
import { sys_version_indexComponent } from './sys_version/index.component';
import { sys_version_history_indexComponent } from './sys_version_history/index.component';
import { sys_mongodb_user_auth_logComponent } from './sys_mongodb_user_auth_log/index.component';
import { popup_detailComponent } from './sys_mongodb_user_auth_log/popup_detail';
import { sys_configuration_indexComponent } from './sys_configuration/index.component';


export const systemsRoutes: Route[] = [
    {
        path: 'sys_help_index',
        component: sys_help_indexComponent,
    },
    {
        path: 'sys_help_detail_index',
        component: sys_help_detail_indexComponent,
    },
    {
        path: 'sys_unit_index',
        component: sys_unit_indexComponent
    },
    {
        path: 'sys_unit_index',
        component: sys_unit_indexComponent
    },
    {
        path: 'sys_delivery_type_index',
        component: sys_delivery_type_indexComponent
    },
    {
        path: 'sys_receiving_type_index',
        component: sys_receiving_type_indexComponent
    },
    {
        path: 'sys_item_type_index',
        component: sys_item_type_indexComponent
    },
    {
        path: 'sys_item_index',
        component: sys_item_indexComponent
    },
    {
        path: 'sys_phase_index',
        component: sys_phase_indexComponent
    },
    {
        path: 'sys_customer_index',
        component: sys_customer_indexComponent
    },
    {
        path: 'sys_item_specification_index',
        component: sys_item_specification_indexComponent
    },
    {
        path: 'sys_item_bom_index',
        component: sys_item_bom_indexComponent
    },
    {
        path: 'sys_group_user_index',
        component: sys_group_user_indexComponent
    },
    {
        path: 'sys_user_index',
        component: sys_user_indexComponent
    },
    {
        path: 'sys_approval_config_index',
        component: sys_approval_config_indexComponent
    },
    {
        path: 'sys_factory_index',
        component: sys_factory_indexComponent
    },
    {
        path: 'sys_factory_line_item_capacity_index',
        component: sys_factory_line_item_capacity_indexComponent
    },

    {
        path: 'sys_factory_line_index',
        component: sys_factory_line_indexComponent
    },
    {
        path: 'sys_department_index',
        component: sys_department_indexComponent
    },
    {
        path: 'sys_job_title_index',
        component: sys_job_title_indexComponent
    },
    {
        path: 'sys_cost_type_index',
        component: sys_cost_type_indexComponent
    },
    {
        path: 'sys_opc_index',
        component: sys_opc_indexComponent
    },
    {
        path: 'sys_opc_view_index',
        component: sys_opc_viewComponent
    },
    {
        path: 'sys_template_opc_index',
        component: sys_template_opc_indexComponent
    },
    {
        path: 'sys_template_opc_param_index',
        component: sys_template_opc_param_indexComponent
    },
    {
        path: 'sys_opc_client_index',
        component: sys_opc_client_indexComponent
    },
    {
        path: 'sys_workstation_index',
        component: sys_workstation_indexComponent
    },
    {
        path: 'sys_workstation_template_opc_index',
        component: sys_workstation_template_opc_indexComponent
    },
    {
        path: 'sys_inventory_position_item_capacity_index',
        component: sys_inventory_position_item_capacity_indexComponent
    },
    {
        path: 'sys_vendor_item_index',
        component: sys_vendor_item_indexComponent
    },
    {
        path: 'sys_work_schedule_factory_line_index',
        component: sys_work_schedule_factory_lineindexComponent
    },
    {
        path: 'sys_version_index',
        component: sys_version_indexComponent
    },
    {
        path: 'sys_version_history_index',
        component: sys_version_history_indexComponent
    },
    {
        path: 'sys_help_page_index',
        component: sys_help_page_indexComponent,
        children : [
            {
                path     : ':id',
                component: sys_help_page_detailComponent,
            },

        ]
    },
    {
        path: 'sys_mongodb_user_auth_log_index',
        component: sys_mongodb_user_auth_logComponent
    },
    {
        path: 'popup_log_detail',
        component: popup_detailComponent

    },
    {
        path: 'sys_configuration_index',
        component: sys_configuration_indexComponent

    }

];

