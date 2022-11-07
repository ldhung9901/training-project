import {
    compactNavigation,
    defaultNavigation,
    futuristicNavigation,
    horizontalNavigation,
} from 'app/mock-api/common/navigation/data';
import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, Resolve, RouterStateSnapshot } from '@angular/router';
import { forkJoin, Observable } from 'rxjs';
import { map, switchMap } from 'rxjs/operators';
import { InitialData } from 'app/app.types';
import { cloneDeep } from 'lodash-es';
import { FuseNavigationItem } from '@fuse/components/navigation';
import { FuseMockApiService } from '@fuse/lib/mock-api';
import { HttpClient } from '@angular/common/http';
import { TranslocoService } from '@ngneat/transloco';
import { of } from 'rxjs';
import { AuthService } from './core/auth/auth.service';
import { Message } from './layout/common/messages/messages.types';

@Injectable({
    providedIn: 'root',
})
export class InitialDataResolver implements Resolve<any> {
    public menu: any;
    private _compactNavigation: FuseNavigationItem[] = compactNavigation;
    private _defaultNavigation: FuseNavigationItem[] = defaultNavigation;
    private _futuristicNavigation: FuseNavigationItem[] = futuristicNavigation;
    private _horizontalNavigation: FuseNavigationItem[] = horizontalNavigation;
    /**
     * Constructor
     */
    constructor(
        private _httpClient: HttpClient,
        private _fuseMockApiService: FuseMockApiService,
        public _translocoService: TranslocoService,
        private _authService: AuthService
    ) { }

    // -----------------------------------------------------------------------------------------------------
    // @ Public methods
    // -----------------------------------------------------------------------------------------------------

    /**
     * Use this resolver to resolve initial mock-api for the application
     *
     * @param route
     * @param state
     */
    resolve(): Observable<InitialData> {
        // Fork join multiple API endpoint calls to wait all of them to finish
        return forkJoin([
            this._httpClient.get<any>('notification.ctr/get_all'),
            this._httpClient.post('/sys_home.ctr/getModule/', {}).pipe(
                switchMap((resp: any) => {
                    this._defaultNavigation = loadNavBar(resp, this._translocoService);

                    // Fill compact navigation children using the default navigation
                    this._compactNavigation.forEach((compactNavItem) => {
                        this._defaultNavigation.forEach((defaultNavItem) => {
                            if (defaultNavItem.id === compactNavItem.id) {
                                compactNavItem.children = defaultNavItem.children;
                            }
                        });
                    });
                    // Fill futuristic navigation children using the default navigation
                    this._futuristicNavigation.forEach((futuristicNavItem) => {
                        this._defaultNavigation.forEach((defaultNavItem) => {
                            if (defaultNavItem.id === futuristicNavItem.id) {
                                futuristicNavItem.children = defaultNavItem.children;
                            }
                        });
                    });
                    // Fill horizontal navigation children using the default navigation
                    this._horizontalNavigation.forEach((horizontalNavItem) => {
                        this._defaultNavigation.forEach((defaultNavItem) => {
                            if (defaultNavItem.id === horizontalNavItem.id) {
                                horizontalNavItem.children = defaultNavItem.children;
                            }
                        });
                    });
                    // Return the response
                    resp = {
                        compact: this._compactNavigation,
                        default: this._defaultNavigation,
                        futuristic: this._futuristicNavigation,
                        horizontal: this._horizontalNavigation,
                    };
                    return of(resp);
                })
            ),
            this._httpClient.get<any>('api/common/notifications'),
            this._httpClient.get<any>('api/common/shortcuts'),
            this._httpClient.post<any>('sys_home.ctr/checkLogin', { accessToken: this._authService.accessToken }).pipe(
                switchMap((resp: any) => {
                    resp = JSON.parse(localStorage.getItem('user'));
                    return of(resp);
                })
            ),
        ]).pipe(
            map(([messages, navigation, notifications, shortcuts, user]) => ({
                messages,
                navigation: {
                    compact: navigation.compact,
                    default: navigation.default,
                    futuristic: navigation.futuristic,
                    horizontal: navigation.horizontal,
                },
                notifications,
                shortcuts,
                user,
            }))
        );
    }
}

export function checkInclueFn(listInclude: any, stringValue: any): boolean {
    let isInclude = false;
    stringValue.every((element) => {
        if (listInclude.includes(element)) {
            isInclude = true;
            return false;
        } else return true;
    });
    return isInclude;
}
export function loadNavBar(resp: any, _translocoService): any {
    const MENU = resp;
    localStorage.setItem('menu_user', JSON.stringify({ data: resp, time: new Date() }));
    const navmodule = [...defaultNavigation];
    let navigationMenu = [];
    for (let i = 0; i < navmodule.length; i++) {
        const menu = MENU.filter((d) => d.menu.module === navmodule[i].id);
        navmodule[i].title = _translocoService.translate(navmodule[i].title);
        if (menu.length > 0) {
            navmodule[i].children = [];

            switch (navmodule[i].module) {
                case 'inventory':
                    navmodule[i].children.push({
                        icon: 'pending_actions',
                        id: 'inventory',
                        title: _translocoService.translate('NAV.inventory_import'),
                        translate: 'report',
                        type: 'collapsable',
                        children: [],
                    }); // 0
                    navmodule[i].children.push({
                        icon: 'event',
                        id: 'inventory',
                        title: _translocoService.translate('NAV.inventory_export'),
                        translate: 'report',
                        type: 'collapsable',
                        children: [],
                    }); // 1
                    navmodule[i].children.push({
                        icon: 'cottage',
                        id: 'inventory',
                        title: _translocoService.translate('NAV.inventory_transfer'),
                        translate: 'report',
                        type: 'collapsable',
                        children: [],
                    }); // 2
                    navmodule[i].children.push({
                        icon: 'description',
                        id: 'inventory',
                        title: _translocoService.translate('NAV.inventory_check'),
                        translate: 'report',
                        type: 'collapsable',
                        children: [],
                    }); // 4
                    navmodule[i].children.push({
                        icon: 'description',
                        id: 'inventory',
                        title: _translocoService.translate('report'),
                        translate: 'report',
                        type: 'collapsable',
                        children: [],
                    }); // 3
                    break;
                case 'system':
                    navmodule[i].children.push({
                        icon: 'admin_panel_settings',
                        id: 'system',
                        title: _translocoService.translate('NAV.sys_customer'),
                        translate: 'product_information',
                        type: 'collapsable',
                        children: [],
                    }); // 0
                    navmodule[i].children.push({
                        icon: 'manage_accounts',
                        id: 'system',
                        title: _translocoService.translate('sys_item'), //sys_item
                        translate: 'user',
                        type: 'collapsable',
                        children: [],
                    }); //1
                    navmodule[i].children.push({
                        icon: 'live_help',
                        id: 'system',
                        title: _translocoService.translate('NAV.sys_warehouse'), //NAV.sys_warehouse
                        translate: 'version',
                        type: 'collapsable',
                        children: [],
                    }); //2
                    navmodule[i].children.push({
                        icon: 'developer_board',
                        id: 'system',
                        title: _translocoService.translate('NAV.production'), //NAV.production
                        translate: 'work_manual',
                        type: 'collapsable',
                        children: [],
                    }); //3
                    navmodule[i].children.push({
                        icon: 'house',
                        id: 'system',
                        title: _translocoService.translate('NAV.sys_user'), //NAV.sys_user
                        translate: 'warehouse',
                        type: 'collapsable',
                        children: [],
                    }); //4
                    navmodule[i].children.push({
                        icon: 'list_alt',
                        id: 'system',
                        title: _translocoService.translate('NAV.work_manual'), //NAV.work_manual
                        translate: 'item',
                        type: 'collapsable',
                        children: [],
                    }); //5
                    navmodule[i].children.push({
                        icon: 'list_alt',
                        id: 'system',
                        title: _translocoService.translate('NAV.version'), //NAV.version
                        translate: 'customer',
                        type: 'collapsable',
                        children: [],
                    }); //6
                    break;
                case 'business':
                    navmodule[i].children.push({
                        icon: 'description',
                        id: 'business',
                        title: _translocoService.translate('report'),
                        translate: 'report',
                        type: 'collapsable',
                        children: [],
                    }); //0
                case 'production':
                    navmodule[i].children.push({
                        icon: 'description',
                        id: 'production',
                        title: _translocoService.translate('report'),
                        translate: 'report',
                        type: 'collapsable',
                        children: [],
                    }); //0
                    break;
                case 'buyorder':
                    navmodule[i].children.push({
                        icon: 'description',
                        id: 'buyorder',
                        title: _translocoService.translate('report'),
                        translate: 'report',
                        type: 'collapsable',
                        children: [],
                    }); //0
                    break;
                case 'maintenance':
                    navmodule[i].children.push({
                        icon: 'engineering',
                        id: 'maintenance',
                        title: _translocoService.translate('category'),
                        translate: 'category',
                        type: 'collapsable',
                        children: [],
                    }); //0
                    navmodule[i].children.push({
                        icon: 'description',
                        id: 'maintenance',
                        title: _translocoService.translate('report'),
                        translate: 'report',
                        type: 'collapsable',
                        children: [],
                    }); //1
                    navmodule[i].children.push({
                        icon: 'description',
                        id: 'maintenance',
                        title: _translocoService.translate('maintence'),
                        translate: 'report',
                        type: 'collapsable',
                        children: [],
                    }); //2
                    navmodule[i].children.push({
                        icon: 'description',
                        id: 'maintenance',
                        title: _translocoService.translate('repair'),
                        translate: 'report',
                        type: 'collapsable',
                        children: [],
                        is_badge: true
                    }); //3
                    break;
                case 'quality':
                    navmodule[i].children.push({
                        icon: 'sentiment_neutral',
                        id: 'quality',
                        title: _translocoService.translate('category'),
                        translate: 'category',
                        type: 'collapsable',
                        children: [],
                    }); //0
                    break;
                case 'tool':

                    navmodule[i].children.push({
                        icon: 'trending_up',
                        id: 'tool',
                        title: _translocoService.translate('NAV.too_increase_ccdc'),
                        translate: 'report',
                        type: 'collapsable',
                        children: [],
                    }); // 0
                    navmodule[i].children.push({
                        icon: 'trending_down',
                        id: 'tool',
                        title: _translocoService.translate('NAV.tool_reduced_ccdc'),
                        translate: 'report',
                        type: 'collapsable',
                        children: [],
                    }); // 1
                    navmodule[i].children.push({
                        icon: 'swap_horiz',
                        id: 'tool',
                        title: _translocoService.translate('NAV.tool_transfer_ccdc'),
                        translate: 'report',
                        type: 'collapsable',
                        children: [],
                    }); // 2
                    navmodule[i].children.push({
                        icon: 'heroicons_outline:document-duplicate',
                        id: 'tool',
                        title: _translocoService.translate('NAV.too_borrow_return_ccdc'),
                        translate: 'report',
                        type: 'collapsable',
                        children: [],
                    }); // 3
                    navmodule[i].children.push({
                        icon: 'heroicons_outline:chart-square-bar',
                        id: 'tool',
                        title: _translocoService.translate('report'),
                        translate: 'report',
                        type: 'collapsable',
                        children: [],
                    }); // 4
                    break;
                default:
                    // console.log(navmodule[i].module);
                    break;
            }

            if (navmodule[i].module === 'inventory') {
                for (let j = 0; j < menu.length; j++) {
                    menu[j].menu.title = _translocoService.translate(menu[j].menu.translate);
                    menu[j].menu.type = 'basic';
                    menu[j].menu.link = menu[j].menu.url;

                    if (
                        checkInclueFn(menu[j].menu.id, [
                            'inventory_receiving',
                            'inventory_item_line_up',
                            'inventory_production_item_schedule',
                        ])
                    ) {
                        navmodule[i].children[0].children.push(menu[j].menu);
                    } else if (
                        checkInclueFn(menu[j].menu.id, [
                            'inventory_delivery',
                            'inventory_need_supplier_schedule',
                            'inventory_request_export',
                        ])
                    ) {
                        navmodule[i].children[1].children.push(menu[j].menu);
                        // console.log(navmodule[i].children[1]);
                    } else if (checkInclueFn(menu[j].menu.id, ['inventory_request_transfer', 'inventory_process_transfer'])) {
                        navmodule[i].children[2].children.push(menu[j].menu);
                    } else if (
                        checkInclueFn(menu[j].menu.id, [
                            'inventory_check_process',
                            'inventory_check_period',
                            'inventory_check_planning',
                            'inventory_handle_difference',
                            'inventory_check_report',
                        ])
                    ) {
                        navmodule[i].children[3].children.push(menu[j].menu);
                    } else if (
                        checkInclueFn(menu[j].menu.id, [
                            'inventory_position_report_import_export',
                            'inventory_position_report_import_export_specification',
                            'inventory_report_import_export',
                            'inventory_report_import_export_specification',
                            'inventory_report_min_max_stock',
                        ])
                    ) {
                        navmodule[i].children[4].children.push(menu[j].menu);
                    } else {
                        navmodule[i].children.push(menu[j].menu);
                    }
                }
            } else if (navmodule[i].module === 'business') {
                for (let j = 0; j < menu.length; j++) {
                    menu[j].menu.title = _translocoService.translate(menu[j].menu.translate);
                    menu[j].menu.type = 'basic';
                    menu[j].menu.link = menu[j].menu.url;
                    if (
                        checkInclueFn(menu[j].menu.id, [
                            'business_report_delivery_item',
                            'business_report_revenue_monthly',
                            'business_dashboard',
                        ])
                    ) {
                        navmodule[i].children[0].children.push(menu[j].menu);
                    } else {
                        navmodule[i].children.push(menu[j].menu);
                    }
                }
            } else if (navmodule[i].module === 'buyorder') {
                for (let j = 0; j < menu.length; j++) {
                    menu[j].menu.title = _translocoService.translate(menu[j].menu.translate);
                    menu[j].menu.type = 'basic';
                    menu[j].menu.link = menu[j].menu.url;
                    if (
                        checkInclueFn(menu[j].menu.id, [
                            'buyorder_report_import_item',
                        ])
                    ) {
                        navmodule[i].children[0].children.push(menu[j].menu);
                    } else {
                        navmodule[i].children.push(menu[j].menu);
                    }
                }
            } else if (navmodule[i].module === 'system') {
                for (let j = 0; j < menu.length; j++) {
                    menu[j].menu.title = _translocoService.translate(menu[j].menu.translate);
                    menu[j].menu.type = 'basic';
                    menu[j].menu.link = menu[j].menu.url;
                    if (
                        checkInclueFn(menu[j].menu.id, [
                            'sys_customer',
                            'sys_vendor_item',
                        ])
                    ) {
                        navmodule[i].children[0].children.push(menu[j].menu);
                    } else if (
                        checkInclueFn(menu[j].menu.id, [
                            'sys_item',
                            'sys_item_specification',
                            'sys_item_type',
                            'sys_unit',
                        ])
                    ) {
                        navmodule[i].children[1].children.push(menu[j].menu);
                    } else if (
                        checkInclueFn(menu[j].menu.id, [
                            'sys_warehouse',
                            'sys_warehouse_position',
                            'sys_inventory_position_item_capacity',
                            'sys_receiving_type',
                            'sys_delivery_type',
                            'sys_cost_type',
                        ])
                    ) {
                        navmodule[i].children[2].children.push(menu[j].menu);
                    } else if (
                        checkInclueFn(menu[j].menu.id, [
                            'sys_factory',
                            'sys_factory_line',
                            'sys_phase',
                            'sys_factory_line_item_capacity',
                            'sys_item_bom',
                            'sys_workstation',
                        ])
                    ) {
                        navmodule[i].children[3].children.push(menu[j].menu);
                    } else if (
                        checkInclueFn(menu[j].menu.id, [
                            'sys_user',
                            'sys_group_user',
                            'sys_department',
                            'sys_job_title',
                            'sys_approval_config',
                        ])
                    ) {
                        navmodule[i].children[4].children.push(menu[j].menu);
                    } else if (
                        checkInclueFn(menu[j].menu.id, [
                            'sys_help',
                            'sys_help_detail',
                            'sys_help_page',
                        ])
                    ) {
                        navmodule[i].children[5].children.push(menu[j].menu);
                    } else if (checkInclueFn(menu[j].menu.id, ['sys_version', 'sys_version_history'])) {
                        navmodule[i].children[6].children.push(menu[j].menu);
                    } else {
                        navmodule[i].children.push(menu[j].menu);
                    }
                }
            } else if (navmodule[i].module === 'maintenance') {
                for (let j = 0; j < menu.length; j++) {
                    menu[j].menu.title = _translocoService.translate(menu[j].menu.translate);
                    menu[j].menu.type = 'basic';
                    menu[j].menu.link = menu[j].menu.url;
                    if (
                        checkInclueFn(menu[j].menu.id, [
                            'maintenance_system',
                            'maintenance_system_device',
                            'maintenance_system_device_detail',
                            'maintenance_system_device_group',
                            'maintenance_error_list',
                            'maintenance_error_reason',
                        ])
                    ) {
                        navmodule[i].children[0].children.push(menu[j].menu);
                    } else if (
                        checkInclueFn(menu[j].menu.id, [
                            'maintenance_report_system',
                            'maintenance_dashboard',
                            'maintenance_report_system_device_detail',
                            'maintenance_report_status_system',
                            'maintenance_report_repair_item',
                            'maintenance_report_machine_stopped_time',
                            'maintenance_report_error_solution',
                        ])
                    ) {
                        navmodule[i].children[1].children.push(menu[j].menu);
                    } else if (
                        checkInclueFn(menu[j].menu.id, [
                            'maintenance_planning',
                            'maintenance_schedual_system_device',
                            'maintenance_process',
                        ])
                    ) {
                        navmodule[i].children[2].children.push(menu[j].menu);
                    } else if (checkInclueFn(menu[j].menu.id, ['maintenance_repair_order', 'maintenance_repair_process'])) {
                        navmodule[i].children[3].children.push(menu[j].menu);
                    } else {
                        navmodule[i].children.push(menu[j].menu);
                    }
                }
            } else if (navmodule[i].module === 'quality') {
                for (let j = 0; j < menu.length; j++) {
                    menu[j].menu.title = _translocoService.translate(menu[j].menu.translate);
                    menu[j].menu.type = 'basic';
                    menu[j].menu.link = menu[j].menu.url;
                    if (checkInclueFn(menu[j].menu.id, ['quality_error_reason', 'quality_solution', 'quality_item_config'])) {
                        navmodule[i].children[0].children.push(menu[j].menu);
                    } else {
                        navmodule[i].children.push(menu[j].menu);
                    }
                }
            } else if (navmodule[i].module === 'production') {
                for (let j = 0; j < menu.length; j++) {
                    menu[j].menu.title = _translocoService.translate(menu[j].menu.translate);
                    menu[j].menu.type = 'basic';
                    menu[j].menu.link = menu[j].menu.url;
                    if (
                        checkInclueFn(menu[j].menu.id, [
                            'production_report',
                            'production_report_supplier',
                            'production_dashboard',
                            'production_report_work_done',
                        ])
                    ) {
                        navmodule[i].children[0].children.push(menu[j].menu);
                    } else {
                        navmodule[i].children.push(menu[j].menu);
                    }
                }
            }
            else if (navmodule[i].module === 'tool') {
                for (let j = 0; j < menu.length; j++) {
                    menu[j].menu.title = _translocoService.translate(menu[j].menu.translate);
                    menu[j].menu.type = 'basic';
                    menu[j].menu.link = menu[j].menu.url;

                    if (
                        checkInclueFn(menu[j].menu.id, [
                            'tool_write_increase',
                        ])
                    ) {
                        navmodule[i].children[0].children.push(menu[j].menu);
                    } else if (
                        checkInclueFn(menu[j].menu.id, [
                            'tool_broken_voucher',
                            'tool_write_reduced',
                        ])
                    ) {
                        navmodule[i].children[1].children.push(menu[j].menu);
                    } else if (checkInclueFn(menu[j].menu.id, ['tool_request_transfer', 'tool_transfer_process'])) {
                        navmodule[i].children[2].children.push(menu[j].menu);
                    } else if (
                        checkInclueFn(menu[j].menu.id, [
                            'tool_write_borrow',
                            'tool_write_return',
                        ])
                    ) {
                        navmodule[i].children[3].children.push(menu[j].menu);
                    } else if (
                        checkInclueFn(menu[j].menu.id, [
                            'tool_logbook_increase_reduced',
                            'tool_diary_borrow_return',
                        ])
                    ) {
                        navmodule[i].children[4].children.push(menu[j].menu);
                    } else {
                        navmodule[i].children.push(menu[j].menu);
                    }
                }
            }

            // get list index of empty module
            let removeValFromIndex = [];
            for (let module = 0; module < 10; module++) {
                if (navmodule[i].children[module]?.children?.length == 0) {
                    removeValFromIndex.push(module);
                }
            }
            // remove empty group
            for (var index = removeValFromIndex.length - 1; index >= 0; index--) {
                navmodule[i].children.splice(removeValFromIndex[index], 1);
            }
            navigationMenu.push(navmodule[i]);
        }
    }

    return navigationMenu;
}

