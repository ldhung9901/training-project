import { inventory_position_report_import_export_db } from "app/modules/inventory/inventory_position_report_import_export/types";


export interface sys_warehouse_position_model {
    createby_name: string;
    warehouse_name: string;
    db: sys_warehouse_position_db;
}
export interface sys_warehouse_position_db {
    id: string;
    id_warehouse: string;
    name: string;
    note: string;
    create_by: string;
    create_date: string | null;
    update_by: string;
    update_date: string | null;
    row: number | null;
    col: number | null;
    status_del: number | null;
}
export interface sys_warehouse_existed_position_model {
    id_specification: number | null;
    id_warehouse_position: string;
    name_warehouse_position: string;
    db: inventory_position_report_import_export_db;
}

export interface received_items {
    id_warehouse: string;
    list_item: item_model[];
}

export interface received_repair_items {
    id_warehouse: string;
    list_item_repair: item_model[];
}

export interface item_model {
    id_item: string;
    id_specification: number | null;
}
