import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { IconDefinition } from '@ant-design/icons-angular';
import * as AllIcons from '@ant-design/icons-angular/icons';
import { NZ_ICONS } from 'ng-zorro-antd/icon';
import { NzIconModule } from 'ng-zorro-antd/icon';

import { NgApexchartsModule } from "ng-apexcharts";
import { sys_unit_indexComponent } from './sys_unit/index.component';

import { MatButtonModule } from '@angular/material/button';
import { MatChipsModule } from '@angular/material/chips';
import { MatRippleModule } from '@angular/material/core';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatSelectModule } from '@angular/material/select';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { MatSortModule } from '@angular/material/sort';
import { MatTableModule } from '@angular/material/table';
import { MatTabsModule } from '@angular/material/tabs';
import { MatDialogModule } from '@angular/material/dialog';
import { MatMenuModule } from '@angular/material/menu';
import { SweetAlert2Module } from '@sweetalert2/ngx-sweetalert2';
import { sys_unit_popUpAddComponent } from './sys_unit/popupAdd.component';
import { sys_item_type_indexComponent } from './sys_item_type/index.component';
import { sys_item_type_popUpAddComponent } from './sys_item_type/popupAdd.component';
import { sys_warehouse_indexComponent } from './sys_warehouse/index.component';
import { sys_warehouse_popUpAddComponent } from './sys_warehouse/popupAdd.component';
import { sys_approval_config_indexComponent } from './sys_approval_config/index.component';
import { sys_approval_config_popUpAddComponent } from './sys_approval_config/popupAdd.component';
import { sys_group_user_indexComponent } from './sys_group_user/index.component';
import { sys_group_user_popUpAddComponent } from './sys_group_user/popupAdd.component';
import { sys_user_indexComponent } from './sys_user/index.component';
import { sys_user_popUpAddComponent } from './sys_user/popupAdd.component';
import { sys_warehouse_position_indexComponent } from './sys_warehouse_position/index.component';
import { sys_warehouse_position_popUpAddComponent } from './sys_warehouse_position/popupAdd.component';
import { NgxMatSelectSearchModule } from 'ngx-mat-select-search';
import { sys_item_indexComponent } from './sys_item/index.component';
import { sys_item_popUpAddComponent } from './sys_item/popupAdd.component';
import { MatCardModule } from '@angular/material/card';
import { sys_phase_indexComponent } from './sys_phase/index.component';
import { sys_phase_popUpAddComponent } from './sys_phase/popupAdd.component';
import { sys_customer_indexComponent } from './sys_customer/index.component';
import { sys_customer_popUpAddComponent } from './sys_customer/popupAdd.component';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { sys_item_specification_indexComponent } from './sys_item_specification/index.component';
import { sys_item_specification_popUpAddComponent } from './sys_item_specification/popupAdd.component';
import { sys_item_bom_indexComponent } from './sys_item_bom/index.component';
import { sys_item_bom_popUpAddComponent } from './sys_item_bom/popupAdd.component';
import { sys_item_bom_popupAdd_configComponent } from './sys_item_bom/popupAddConfig.component';
import { sys_receiving_type_indexComponent } from './sys_receiving_type/index.component';
import { sys_receiving_type_popUpAddComponent } from './sys_receiving_type/popupAdd.component';
import { sys_delivery_type_indexComponent } from './sys_delivery_type/index.component';
import { sys_delivery_type_popUpAddComponent } from './sys_delivery_type/popupAdd.component';
import { sys_factory_indexComponent } from './sys_factory/index.component';
import { sys_factory_line_item_capacity_indexComponent } from './sys_factory_line_item_capacity/index.component';
import { sys_factory_line_item_capacity_popUpAddComponent } from './sys_factory_line_item_capacity/popupAdd.component';
import { sys_factory_popUpAddComponent } from './sys_factory/popupAdd.component';
import { sys_factory_line_indexComponent } from './sys_factory_line/index.component';
import { sys_factory_line_popUpAddComponent } from './sys_factory_line/popupAdd.component';
import { sys_department_indexComponent } from './sys_department/index.component';
import { sys_department_popUpAddComponent } from './sys_department/popupAdd.component';
import { sys_job_title_indexComponent } from './sys_job_title/index.component';
import { sys_job_title_popUpAddComponent } from './sys_job_title/popupAdd.component';
import { sys_cost_type_indexComponent } from './sys_cost_type/index.component';
import { sys_cost_type_popUpAddComponent } from './sys_cost_type/popupAdd.component';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { EditorModule, TINYMCE_SCRIPT_SRC } from '@tinymce/tinymce-angular';
import { sys_item_bom_treeviewComponent } from './sys_item_bom/treeview.component';
import { MatTreeModule } from '@angular/material/tree';
import { sys_opc_indexComponent } from './sys_opc/index.component';
import { sys_opc_popUpAddComponent } from './sys_opc/popupAdd.component';
import { sys_opc_viewComponent } from './sys_opc_view/index.component';
import { sys_template_opc_indexComponent } from './sys_template_opc/index.component';
import { sys_template_opc_popUpAddComponent } from './sys_template_opc/popupAdd.component';
import { sys_template_opc_param_indexComponent } from './sys_template_opc_param/index.component';
import { sys_template_opc_param_popUpAddComponent } from './sys_template_opc_param/popupAdd.component';
import { sys_opc_client_indexComponent } from './sys_opc_client/index.component';
import { sys_opc_client_popUpAddComponent } from './sys_opc_client/popupAdd.component';
import { sys_work_schedule_factory_lineindexComponent } from './sys_work_schedule_factory_line/index.component'
import { sys_workstation_indexComponent } from './sys_workstation/index.component';
import { sys_workstation_popUpAddComponent } from './sys_workstation/popupAdd.component';
import { sys_workstation_template_opc_indexComponent } from './sys_workstation_template_opc/index.component';
import { sys_workstation_template_opc_popUpAddComponent } from './sys_workstation_template_opc/popupAdd.component';
import { sys_inventory_position_item_capacity_indexComponent } from './sys_inventory_position_item_capacity/index.component';
import { sys_inventory_position_item_capacity_popUpAddComponent } from './sys_inventory_position_item_capacity/popupAdd.component';
import { sys_vendor_item_indexComponent } from './sys_vendor_item/index.component';
import { sys_vendor_item_popUpAddComponent } from './sys_vendor_item/popupAdd.component';
import { NzEmptyModule } from 'ng-zorro-antd/empty';
import { NzTreeModule } from 'ng-zorro-antd/tree';
import { NzCheckboxModule } from 'ng-zorro-antd/checkbox';
import { systemsRoutes } from './system.routing';
import { TranslocoModule } from '@ngneat/transloco';
import { FusePipesModule } from '@fuse/pipes/pipes.module';
import { FormsModule } from '@angular/forms';
import { commonModule } from '@fuse/components/commonComponent/common.module';
import { ApprovalModule } from '@fuse/components/commonComponent/approval.module';
import { CommonModule, registerLocaleData } from '@angular/common';
import { SharedModule } from 'app/shared/shared.module';
import { NZ_I18N, vi_VN } from 'ng-zorro-antd/i18n';
import vi from '@angular/common/locales/vi';
import { sys_help_detail_popUpAddComponent } from './sys_help_detail/popupAdd.component';
import { sys_help_indexComponent } from './sys_help/index.component';
import { sys_help_popUpAddComponent } from './sys_help/popupAdd.component';
import { sys_help_detail_indexComponent } from './sys_help_detail/index.component';
import { FuseFindByKeyPipeModule } from '@fuse/pipes/find-by-key';
import { sys_help_page_indexComponent } from './sys_help_page/index.component';
import { sys_help_page_detailComponent } from './sys_help_page/detail.component';
import { CalendarModule } from '@fuse/components/commonComponent/calendar/calendar.module';
import { sys_version_indexComponent } from './sys_version/index.component';
import { sys_version_popUpAddComponent } from './sys_version/popupAdd.component';
import { sys_version_history_indexComponent } from './sys_version_history/index.component';
import { sys_mongodb_user_auth_logComponent } from './sys_mongodb_user_auth_log/index.component';
import { popup_detailComponent } from './sys_mongodb_user_auth_log/popup_detail';
import { common_history_log_form_popUpComponent } from './sys_mongodb_user_auth_log/common_history_log_form_popup';
import { sys_item_popUpAddDetailComponent } from './sys_item/popupAddDetail.component';
import { sys_configuration_indexComponent } from './sys_configuration/index.component';
import { sys_company_infoComponent } from './sys_configuration/sys_company_info/index.component';
import { sys_item_bom_detail_indexComponent } from './sys_item_bom/sys_item_bom_detail/index.component';
import { sys_item_bom_detail_quality_indexComponent } from './sys_item_bom/sys_item_bom_detail/sys_item_bom_detail_quality/index.component';
import { sys_item_bom_detail_popUpAddComponent } from './sys_item_bom/sys_item_bom_detail/popupAdd.component';
import { sys_format_license_configComponent } from './sys_configuration/sys_format_license_config/index.component';
import { sys_working_time_config_popUpAddComponent } from './sys_configuration/sys_work_time_config/popupAdd.component';
import { sys_working_time_config_indexComponent } from './sys_configuration/sys_work_time_config/index.component';
import { MatSidenavModule } from '@angular/material/sidenav';
// import { BOMService } from './sys_item_bom/sys_item_bom.service';

@NgModule({
    declarations: [
        sys_help_indexComponent,
        sys_help_detail_popUpAddComponent,
        sys_help_popUpAddComponent,
        sys_help_detail_indexComponent,
        sys_inventory_position_item_capacity_indexComponent,
        sys_inventory_position_item_capacity_popUpAddComponent,
        sys_item_specification_indexComponent,
        sys_item_specification_popUpAddComponent,
        sys_item_bom_indexComponent,
        sys_item_bom_treeviewComponent,
        sys_item_bom_popUpAddComponent,
        sys_item_bom_popupAdd_configComponent,
        sys_item_bom_detail_indexComponent,
        sys_item_bom_detail_popUpAddComponent,
        sys_item_bom_detail_quality_indexComponent,
        sys_customer_indexComponent,
        sys_opc_indexComponent,
        sys_opc_popUpAddComponent,
        sys_customer_popUpAddComponent,
        sys_phase_indexComponent,
        sys_phase_popUpAddComponent,
        sys_unit_indexComponent,
        sys_help_page_indexComponent,
        sys_unit_popUpAddComponent,
        sys_item_type_indexComponent,
        sys_item_type_popUpAddComponent,
        sys_receiving_type_indexComponent,
        sys_receiving_type_popUpAddComponent,
        sys_delivery_type_indexComponent,
        sys_delivery_type_popUpAddComponent,
        sys_item_indexComponent,
        sys_item_popUpAddComponent,
        sys_item_popUpAddDetailComponent,
        sys_warehouse_indexComponent,
        sys_warehouse_popUpAddComponent,
        sys_approval_config_indexComponent,
        sys_approval_config_popUpAddComponent,
        sys_group_user_indexComponent,
        sys_group_user_popUpAddComponent,
        sys_user_indexComponent,
        sys_work_schedule_factory_lineindexComponent,
        sys_user_popUpAddComponent,
        sys_warehouse_position_indexComponent,
        sys_warehouse_position_popUpAddComponent,
        sys_factory_indexComponent,
        sys_factory_popUpAddComponent,
        sys_factory_line_indexComponent,
        sys_factory_line_popUpAddComponent,
        sys_factory_line_item_capacity_indexComponent,
        sys_factory_line_item_capacity_popUpAddComponent,
        sys_department_indexComponent,
        sys_department_popUpAddComponent,
        sys_job_title_indexComponent,
        sys_job_title_popUpAddComponent,
        sys_cost_type_indexComponent,
        sys_cost_type_popUpAddComponent,
        sys_opc_viewComponent,
        sys_template_opc_indexComponent,
        sys_template_opc_popUpAddComponent,
        sys_template_opc_param_indexComponent,
        sys_template_opc_param_popUpAddComponent,
        sys_opc_client_indexComponent,
        sys_opc_client_popUpAddComponent,
        sys_workstation_indexComponent,
        sys_workstation_popUpAddComponent,
        sys_workstation_template_opc_indexComponent,
        sys_workstation_template_opc_popUpAddComponent,
        sys_vendor_item_indexComponent,
        sys_vendor_item_popUpAddComponent,
        sys_help_detail_popUpAddComponent,
        sys_help_page_detailComponent,
        sys_version_indexComponent,
        sys_version_popUpAddComponent,
        sys_version_history_indexComponent,
        sys_mongodb_user_auth_logComponent,
        popup_detailComponent,
        common_history_log_form_popUpComponent,
        sys_configuration_indexComponent,
        sys_company_infoComponent,
        sys_format_license_configComponent,
        sys_working_time_config_indexComponent,
        sys_working_time_config_popUpAddComponent,


    ],
    imports: [
        RouterModule.forChild(systemsRoutes),
        NzEmptyModule,
        SweetAlert2Module,
        MatButtonModule,
        MatChipsModule,
        MatExpansionModule,
        MatFormFieldModule,
        MatIconModule,
        MatInputModule,
        MatMenuModule,
        MatPaginatorModule,
        MatTreeModule,
        MatRippleModule,
        NzTreeModule,
        MatSelectModule,
        MatCheckboxModule,
        MatSortModule,
        MatSnackBarModule,
        MatTableModule,
        MatTabsModule,
        MatDialogModule,
        FusePipesModule,
        FuseFindByKeyPipeModule,
        MatCardModule,
        NgxMatSelectSearchModule,
        commonModule,
        CommonModule,
        ApprovalModule,
        NzCheckboxModule,
        NzIconModule,
        TranslocoModule,
        FusePipesModule,
        FormsModule,
        SharedModule,
        EditorModule,
        CalendarModule,
        MatSidenavModule
    ],
    exports: [

    ],
    providers: [
        { provide: TINYMCE_SCRIPT_SRC, useValue: 'tinymce/tinymce.min.js' },
        { provide: NZ_I18N, useValue: vi_VN },
    ]

})

export class SystemModule {
}
