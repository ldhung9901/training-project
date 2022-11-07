export interface ItemOtherUnit {
    id: string;
    name: string;
    conversion_factor: number;
}
export interface ItemSpecification {
    id: number;
    name: string;
    conversion_factor: number;
    id_unit: string;
    sys_unit_name: string;
}

export interface ItemBackend {
    id: string;
    name: string;
    id_unit: string;
    unit_name: string;
    code_item: string;
}

export interface sys_item_db {
    id: string;
    price: number | null;
    id_item_type: string;
    name: string;
    note: string;
    pathImg: string;
    type: number | null;
    id_unit: string;
    id_cost_type: string;
    status_del: number | null;
    create_by: string;
    create_date: string | null;
    update_by: string;
    update_date: string | null;
    has_bom: number | null;
    has_specification: number | null;
    code_item: string;
    description: string;
}
export interface sys_item_model {
    createby_name: string;
    sys_item_type_name: string;
    sys_unit_name: string;
    sys_cost_type_name: string;
    db: sys_item_db;
    list_item_quality: sys_item_quality[];
    list_sys_item_unit_other: sys_item_unit_other_model[];
    list_item_min_max_stock: sys_item_min_max_stock_model[];
    type_name: string;
    sys_cost_type_code: string;
    inventory_quantity: number;
}

export interface sys_item_unit_other_db {
    id: number;
    id_unit: string;
    id_item: string;
    conversion_factor: number | null;
    note: string;
}
export interface sys_item_unit_other_model {
    sys_unit_name: string;
    db: sys_item_unit_other_db;
}
export interface sys_item_min_max_stock_db {
    id: number;
    id_item: string;
    id_warehouse: string;
    min_stock: number | null;
    max_stock: number | null;
}


export interface sys_item_min_max_stock_model {
    sys_warehouse_name: string;
    db: sys_item_min_max_stock_db;
}
export interface sys_item_quality {
    actionEnum?:number,
    db?: {
        id?: number;
        id_item?: string;
        config_code?: string;
        config_name?: string;
        aplly_date?: string | null;
        note?: string;
        status_use?: boolean;
        status_del?: number | null;
        create_by?: string;
        create_date?: string | null;
        update_by?: string;
        update_date?: string | null;
    }
    list_item_quality?: sys_item_quality_detail_db[]

}
export interface sys_item_quality_detail_db {
    id?: number;
    id_sys_item_detail?: string;
    config_content?: string;
    description?: string;
    type_evaluate?: string;
    number_evaluate?: number;
    error_range_start?: number;
    error_range_end?: number;
    status_del?: number | null;
    create_by?: string;
    create_date?: string | null;
    update_by?: string;
    update_date?: string | null;
}
