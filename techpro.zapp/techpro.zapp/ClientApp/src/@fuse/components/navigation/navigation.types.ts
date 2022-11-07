import { IsActiveMatchOptions } from '@angular/router';

export interface ResponseNavigation {
    badge_return: number
    badge_approval: number
    id: string
    title: string
    url: string
    icon: string
    translate: string
    type: string
    controller: string
    module: string
    is_badge: boolean
    is_approval: boolean
    is_show_all_user: boolean
    badge_version: any
    list_role: ListRole[]
    list_controller_action_public: string[]
  }

  export interface ListRole {
    id: string
    name: string
    list_controller_action: string[]
  }

export interface FuseNavigationItem
{
    id?: string;
    title?: string;
    subtitle?: string;
    module?:string;
    type:
        | 'aside'
        | 'basic'
        | 'collapsable'
        | 'divider'
        | 'group'
        | 'spacer';
    hidden?: (item: FuseNavigationItem) => boolean;
    active?: boolean;
    translate: string;
    disabled?: boolean;
    link?: string;
    externalLink?: boolean;
    exactMatch?: boolean;
    isActiveMatchOptions?: IsActiveMatchOptions;
    function?: (item: FuseNavigationItem) => void;
    classes?: {
        title?: string;
        subtitle?: string;
        icon?: string;
        wrapper?: string;
    };
    icon?: string;
    badge_approval?:number;
    badge_version?:string;
    badge_return?:number;
    is_badge?:boolean;
    is_approval?:boolean;
    children?: FuseNavigationItem[];
    meta?: any;
}

export type FuseVerticalNavigationAppearance =
    | 'default'
    | 'compact'
    | 'dense'
    | 'thin';

export type FuseVerticalNavigationMode =
    | 'over'
    | 'side';

export type FuseVerticalNavigationPosition =
    | 'left'
    | 'right';
