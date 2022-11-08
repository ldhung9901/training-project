using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using techpro.DataBase.Function;
using techpro.DataBase.System;

namespace techpro.DataBase.Provider
{
    public partial class techproDefautContext
    {
        public virtual DbSet<sys_unit_db> sys_units { get; set; }
        public virtual DbSet<sys_phase_db> sys_phases { get; set; }
        public virtual DbSet<sys_customer_db> sys_customers { get; set; }
        
        public virtual DbSet<sys_factory_db> sys_factorys { get; set; }
        public virtual DbSet<sys_factory_line_db> sys_factory_lines { get; set; }
        public virtual DbSet<sys_factory_line_item_capacity_db> sys_factory_line_item_capacitys { get; set; }
        public virtual DbSet<sys_approval_config_db> sys_approval_configs { get; set; }
        public virtual DbSet<sys_approval_config_detail_db> sys_approval_config_details { get; set; }
        public virtual DbSet<sys_group_user_db> sys_group_users { get; set; }
        public virtual DbSet<sys_group_user_detail_db> sys_group_user_details { get; set; }
        public virtual DbSet<sys_group_user_role_db> sys_group_user_roles { get; set; }
        public virtual DbSet<sys_item_type_db> sys_item_types { get; set; }
        public virtual DbSet<sys_delivery_type_db> sys_delivery_types { get; set; }
        public virtual DbSet<sys_receiving_type_db> sys_receiving_types { get; set; }
        public virtual DbSet<sys_work_schedule_factory_line_db> sys_work_schedule_factory_lines { get; set; }
        public virtual DbSet<sys_item_db> sys_items { get; set; }
        public virtual DbSet<sys_item_quality_db> sys_item_quanlitys { get; set; }
        public virtual DbSet<sys_item_quality_detail_db>sys_item_quanlity_details { get; set; }
        public virtual DbSet<sys_item_specification_db> sys_item_specifications { get; set; }
        public virtual DbSet<sys_inventory_position_item_capacity_db> sys_inventory_position_item_capacitys { get; set; }
        public virtual DbSet<sys_item_bom_db> sys_item_boms { get; set; }
        public virtual DbSet<sys_item_bom_config_db> sys_item_bom_configs { get; set; }
        public virtual DbSet<sys_item_bom_quality_db> sys_item_bom_qualitys { get; set; }
        public virtual DbSet<sys_item_unit_other_db> sys_item_unit_others { get; set; }

        public virtual DbSet<sys_approval_detail_db> sys_approval_details { get; set; }
        public virtual DbSet<sys_approval_db> sys_approvals { get; set; }
        public virtual DbSet<sys_approval_step_db> sys_approval_steps { get; set; }
        public virtual DbSet<sys_department_db> sys_departments { get; set; }
        public virtual DbSet<sys_job_title_db> sys_job_titles { get; set; }
        public virtual DbSet<sys_item_min_max_stock_db> sys_item_min_max_stocks { get; set; }
        public virtual DbSet<sys_cost_type_db> sys_cost_types { get; set; }

        public virtual DbSet<sys_opc_db> sys_opcs { get; set; }
        public virtual DbSet<sys_template_opc_db> sys_template_opcs { get; set; }
        public virtual DbSet<sys_template_opc_param_db> sys_template_opc_params { get; set; }
        public virtual DbSet<sys_opc_client_db> sys_opc_clients { get; set; }
        public virtual DbSet<sys_workstation_db> sys_workstations { get; set; }
        public virtual DbSet<sys_workstation_template_opc_db> sys_workstation_template_opcs { get; set; }
        public virtual DbSet<sys_vendor_item_db> sys_vendor_items { get; set; }
        public virtual DbSet<sys_vendor_item_history_db> sys_vendor_item_historys { get; set; }

        public virtual DbSet<sys_factory_line_list_maintenance_system_db> sys_factory_line_list_maintenance_systems { get; set; }

        public virtual DbSet<sys_machine_opc_runtime_db> sys_machine_opc_runtimes { get; set; }
        public virtual DbSet<User> users { get; set; }
         public virtual DbSet<sys_help_db> sys_helps { get; set; }
        public virtual DbSet<sys_help_detail_db> sys_help_details { get; set; }
        public virtual DbSet<sys_version_db> sys_versions { get; set; }
        public virtual DbSet<sys_file_manager_db> sys_file_managers { get; set; }
        public virtual DbSet<sys_company_db> sys_companys { get; set; }
        public virtual DbSet<sys_format_license_config_db> sys_format_license_configs { get; set; }
        public virtual DbSet<sys_working_time_config_db> sys_working_time_configs { get; set; }
        public virtual DbSet<sys_working_time_detail_config_db> sys_working_time_detail_configs { get; set; }

        protected void systemTableBuilder(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<sys_unit_db>(entity =>
           {

           });
            modelBuilder.Entity<User>(entity =>
           {

           });
            modelBuilder.Entity<Fn_get_sys_approval>(entity =>
            {

            });
            OnModelCreatingPartial(modelBuilder);
        }





    }
}
