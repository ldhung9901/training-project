export interface sys_warehouse_model {
    createby_name: string;
    db: sys_warehouse_db;
}

export interface sys_warehouse_db {
    id: string;
    name: string;
    note: string;
    location: string;
    status_del: number | null;
    create_by: string;
    create_date: string | null;
    update_by: string;
    update_date: string | null;
}
