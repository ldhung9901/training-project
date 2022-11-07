import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';

import { HttpClient } from '@angular/common/http';
import { BasePopUpAddComponent } from 'app/Basecomponent/BasePopupAdd.component';
import { TranslocoService } from '@ngneat/transloco';
import { FuseConfirmationService } from '@fuse/services/confirmation';
import { ActivatedRoute } from '@angular/router';
import { NzTableQueryParams } from 'ng-zorro-antd/table';
import moment from 'moment';
import _ from 'lodash';
import { mongodb_user_log_detail_db, mongodb_user_log_detail_map } from 'app/modules/system/sys_mongodb_user_auth_log/types';




@Component({
    selector: 'common_history_log_form_popUp',
    templateUrl: 'common_history_log_form_popup.html',
})
export class common_history_log_form_popUpComponent {
    public filter={ search: "",to_date: new Date, end_date: new Date };
    public pageSize = 10;
    public pageIndex = 1;
    public sortField: string;
    public total = 1;
    public listData: mongodb_user_log_detail_db[]=[];
    public pageLoading: Boolean = true;
    public currentIndex: number;
    public listMapTree: mongodb_user_log_detail_map[] = [];
    public listDataMap:mongodb_user_log_detail_map[]=[];
    public controller:string="";
    constructor(public dialogRef: MatDialogRef<common_history_log_form_popUpComponent>,
        public http: HttpClient, _translocoService: TranslocoService,
        fuseConfirmationService: FuseConfirmationService, route: ActivatedRoute,
        @Inject('BASE_URL') baseUrl: string,
        @Inject(MAT_DIALOG_DATA) data: any) {
        this.controller=data;
    }
    public rerender(): void {
        if (this.filter.search != "" && this.filter.search.trim() != "") {
            this.pageSize = 10;
            this.pageIndex = 1;
        }
        this.loadData(this.pageIndex, this.pageSize);
    };

    loadData(
        pageIndex: number,
        pageSize: number) {
        this.pageLoading = true;
        this.http
            .post(this.controller+'.ctr/GetHistory/',
                {
                    param1: { pageSize, pageIndex },
                    data: this.filter
                }
            ).subscribe((resp: any) => {
                this.listDataMap=[];
                this.listMapTree=[];
                resp.data.forEach(element => {
                    let item: mongodb_user_log_detail_map = {
                        key: element.id.toString(),
                        name: moment(element.create_date).format('DD/MM/YYYY'),
                        db: element
                    }
                    this.listDataMap.push(item);
                });

                 const  groups = _.groupBy(this.listDataMap, t=>t.name);

                 let key = Object.keys(groups);

                 this.listDataMap = key.map(e => {
                    let item:mongodb_user_log_detail_map = {
                      key: e,
                      name: e,
                      children: groups[e],
                    }
                    return item;
                  })

                this.listDataMap.forEach(item => {
                    this.listMapTree[item.key] = this.convertTreeToList(item);
                });

                this.total = resp.recordsTotal;
                this.currentIndex = resp.start;
                this.pageLoading = false;
            });
    }

    onQueryParamsChange(params: NzTableQueryParams): void {
        this.pageSize = params.pageSize;
        this.pageIndex = params.pageIndex;
        this.loadData(this.pageIndex, this.pageSize);
    }

    collapse(array: mongodb_user_log_detail_map[], data: mongodb_user_log_detail_map, $event: boolean): void {
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

    convertTreeToList(root: mongodb_user_log_detail_map): mongodb_user_log_detail_map[] {
        const stack: mongodb_user_log_detail_map[] = [];
        const array: mongodb_user_log_detail_map[] = [];
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

    visitNode(node: mongodb_user_log_detail_map, hashMap: { [key: string]: boolean }, array: mongodb_user_log_detail_map[]): void {
        if (!hashMap[node.key]) {
            hashMap[node.key] = true;
            array.push(node);
        }
    }

}
