export interface mongodb_user_log_detail_db {
    id?: string;
    id_user_auth_log?: string;
    controller_name?: string;
    action_name?: string;
    request_work?: string;
    request_body_data?: string;
    response_data?: string;
    request_time?: string | null;
    create_date?: string | null;
    device_ip_address?: string;
    user_name?: string;
    account?: string;
    user_id?: string;
    device_name?: string;
    voucher_number?: string;
}

export interface mongodb_user_log_detail_map {
    name:string;
    key:string;
    children?:mongodb_user_log_detail_map[];
    level?: number;
    expand?: boolean;
    parent?: mongodb_user_log_detail_map;
    db?:mongodb_user_log_detail_db
}
