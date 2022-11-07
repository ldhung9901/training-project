
export interface Message {
    id?: string;
    id_user?: string;
    module?: string;
    title?: string;
    content?: string;
    url?: string;
    data?: string;
    type?: string;
    color?: string;
    status_del?: number | null;
    is_read?: boolean | null;
    create_date?: string;
    create_by?: string;
}
