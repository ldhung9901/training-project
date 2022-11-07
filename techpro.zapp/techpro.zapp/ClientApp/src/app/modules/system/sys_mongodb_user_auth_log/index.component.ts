import { Component, Inject, ViewContainerRef } from '@angular/core';


import { HttpClient } from '@angular/common/http';



import { TranslocoService } from '@ngneat/transloco';
import { MatDialog } from '@angular/material/dialog';


import { BaseIndexDatatableComponent } from 'app/Basecomponent/BaseIndexDatatable.component';

import { FuseConfirmationService } from '@fuse/services/confirmation';
import { ActivatedRoute } from '@angular/router';

import _, { keyBy } from 'lodash';
import { popup_detailComponent } from './popup_detail';
import moment from 'moment';
import { CmSelectType } from '@fuse/components/commonComponent/cm_select_component/types';
import { Overlay } from '@angular/cdk/overlay';
export interface TreeNodeInterface {
    key: string;
    name: string;
    age?: number;
    level?: number;
    expand?: boolean;
    address?: string;
    children?: TreeNodeInterface[];
    parent?: TreeNodeInterface;
}

@Component({
    selector: 'sys_mongodb_user_auth_log_index',
    templateUrl: './index.component.html',
})

export class sys_mongodb_user_auth_logComponent extends BaseIndexDatatableComponent {
    public list_opc_client: any;
    public listStatusDel: CmSelectType[];
    public listMapTree: any = [];
    optionDateSelectedId: number;
    disabledOptionDate: boolean;

    constructor(http: HttpClient, dialog: MatDialog
        , _translocoService: TranslocoService
        , @Inject('BASE_URL') baseUrl: string, _overlay: Overlay, _viewContainerRef: ViewContainerRef
        , fuseConfirmationService: FuseConfirmationService, route: ActivatedRoute
    ) {
        super(http, baseUrl, _translocoService, fuseConfirmationService, route, dialog, 'sys_mongodb_user_auth_log',
            {
                search: "", range: {
                    type: 5,
                    start: moment().startOf('month').toDate(),
                    end: moment().endOf('month').toDate(),
                }
            }, _overlay, _viewContainerRef
        )


    }

    collapse(array: TreeNodeInterface[], data: TreeNodeInterface, $event: boolean): void {
        if (!$event) {
            if (data.children) {
                data.children.forEach(d => {
                    const target = array.find(a => a.key === d.key)!;
                    target.expand = false;
                    this.collapse(array, target, false);
                });
            } else {
                return;
            }
        }
    }

    convertTreeToList(root: TreeNodeInterface): TreeNodeInterface[] {
        const stack: TreeNodeInterface[] = [];
        const array: TreeNodeInterface[] = [];
        const hashMap = {};
        stack.push({ ...root, level: 0, expand: true });

        while (stack.length !== 0) {
            const node = stack.pop()!;
            this.visitNode(node, hashMap, array);
            if (node.children) {
                for (let i = node.children.length - 1; i >= 0; i--) {
                    stack.push({ ...node.children[i], level: node.level! + 1, expand: false, parent: node });
                }
            }
        }

        return array;
    }

    visitNode(node: TreeNodeInterface, hashMap: { [key: string]: boolean }, array: TreeNodeInterface[]): void {
        if (!hashMap[node.key]) {
            hashMap[node.key] = true;
            array.push(node);
        }
    }

    ngOnInit(): void {

    }

    selectFilter() {
        this.pageSize = 10;
        this.pageIndex = 1;
        this.rerender();
    }

    handleDataBefor() {

        this.listData.map(e => {
            e.date = moment(e.login_time).format('DD/MM/YYYY');
            e.key = e.id;
            return e;
        });

        var groups = _.groupBy(this.listData, 'date');

        let key = Object.keys(groups);
        this.listData = key.map(e => {
            let item = {
                key: e,
                name: e,
                children: groups[e],
            }
            return item;
        })

        this.listData.forEach(item => {
            this.listMapTree[item.key] = this.convertTreeToList(item);
        });

    }

    openDialogDetail(id): void {
        const dialogRef = this.dialog.open(popup_detailComponent, {
            disableClose: true,
            data: {
                id_user_auth_log: id,
            }
        });


        dialogRef.afterClosed().subscribe(result => {

        });
    }

    cleanFilter() {
        this.filter = {
            search: "", range: {
                type: 5,
                start: moment().startOf('month').toDate(),
                end: moment().endOf('month').toDate(),
            }
        };

        this.optionDateSelectedId = 0;
        this.disabledOptionDate = false;
    }
}


