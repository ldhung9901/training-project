
export interface sys_company_model {
    createby_name?: string;
    updateby_name?: string;
    db: sys_company_db;
}

export interface sys_company_db {
    id?: string;
    name?: string;
    address?: string;
    phone?: string;
    tax_number?:string;
    representative?: string;
    create_date?: string | null;
    create_by?: string;
    update_by?: string;
    status_del?: number | null;
    update_date?: string | null;
}
