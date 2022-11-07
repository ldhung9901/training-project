export interface sys_item_bom_model {
    actionEnum?: number;
    index_string?: string;
    quota_cal?: number | null;
    createby_name?: string;
    sys_item_code?: string;
    sys_item_name?: string;
    sys_config_name?: string;
    sys_item_bom_unit_name?: string;
    sys_item_bom_name?: string;
    specification_name?: string;
    phase_name?: string;
    db?: sys_item_bom_db;
}

export interface sys_item_bom_db {
    id?: number;
    id_item?: string;
    id_item_bom_config?: number | null;
    id_item_bom?: string;
    id_phase?: string;
    id_specification?: number | null;
    id_unit?: string;
    id_unit_main?: string;
    quota: number | null;
    quota_main?: number | null;
    conversion_factor?: number | null;
    create_by?: string;
    create_date?: string | null;
    update_by?: string;
    update_date?: string | null;
    note: string;
}
export interface sys_item_bom_config_model {
    list_item_quota?: sys_item_bom_model[];
    list_item_quality?: sys_item_bom_quality_model[];
    actionEnum?: number;
    sys_item_name?: string;
    sys_unit_name?: string;
    specification_name?: string;
    db?: sys_item_bom_config_db;
}

export interface sys_item_bom_config_db {
    id?: number;
    id_item: string;
    config_code: string;
    id_specification?: number | null;
    status_del?: number | null;
    id_unit: string;
    create_by?: string;
    create_date?: string | null;
    update_by?: string;
    update_date?: string | null;
    note: string;
    name: string;
    status_use: boolean;
    aplly_date: string;
}
export interface sys_item_bom_quality_model {
    db?: sys_item_bom_quality_db,
    phase_name?: string
}
export interface sys_item_bom_quality_db {
    id?: number;
    id_item: string;
    id_item_bom_config: number;
    id_phase: string;
    content: string;
    description: string;
    type_evaluate: string;
    number_evaluate?: string;
    error_range_start?: number | null;
    error_range_end?: number | null;
    status_del?: number | null;
    create_by?: string;
    create_date?: string | null;
    update_by?: string;
    update_date?: string | null;
}
