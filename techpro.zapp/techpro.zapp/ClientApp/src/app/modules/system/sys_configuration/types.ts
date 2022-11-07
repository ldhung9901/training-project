


export interface sys_configuration_model {
    createby_name?: string;
    db_company?: sys_company_db;
}

export interface sys_configuration_model_v2 {
    createby_name?: string;
    first_character?: string;
    menu?: string;
    is_raise?: boolean | null;
    status_del?:  number | null;
    db_format_license_config?: sys_format_license_config_db[];
}


export interface sys_working_time_model {
    createby_name?: string;
    actionEnum?: number;
    name?: string;
    schedual_date?: number;
    note?: string;
    list_schedule?: sys_working_time_detail_model[];
    db?: sys_working_time_config_db;
}

export interface sys_working_time_detail_model {
    createby_name?: string;
    db?: sys_working_time_detail_config_db;
}

export interface sys_company_db {
    id?: string;
    name?: string;
    address?: string;
    phone?: string;
    tax_number?: string;
    pathimg?: string;
    user_service?: string;
    create_date?: string | null;
    create_by?: string;
    update_by?: string;
    status_del?: number | null;
    update_date?: string | null;
}

export interface sys_format_license_config_db {
    id?: number;
    first_character?: string;
    is_raise?: boolean | null;
    menu?: string;
    receiptCode?: string;
    create_date?: string | null;
    create_by?: string;
    update_by?: string;
    status_del?: number | null;
    update_date?: string | null;
}

export interface sys_working_time_config_db {
    id?: string;
    name?: string;
    schedual_date?: string | null;
    create_by?: string;
    create_date?: string;
    update_by?: string;
    update_date?: string | null;
    status_del?: number | null;
    note: string;
}


export interface sys_working_time_detail_config_db {
    id?: number;
    create_by?: string;
    create_date?: string | null;
    shift_1?: number;
    shift_2?: number;
    is_off?: boolean | null;
    estimate_check_date?: string | null;
    update_by?: string;
    update_date?: string | null;
    note?: string;
}
