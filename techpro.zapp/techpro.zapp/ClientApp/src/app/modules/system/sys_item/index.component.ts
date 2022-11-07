import { Component, Inject, ViewContainerRef } from '@angular/core';


import { HttpClient} from '@angular/common/http';



import { TranslocoService } from '@ngneat/transloco';
import { sys_item_popUpAddComponent } from './popupAdd.component';
import { MatDialog } from '@angular/material/dialog';


import { BaseIndexDatatableComponent } from 'app/Basecomponent/BaseIndexDatatable.component';

import { FuseConfirmationService } from '@fuse/services/confirmation';
import { ActivatedRoute } from '@angular/router';
import { actionEnums } from 'app/Basecomponent/actionEnums';
import { CmSelectType } from '@fuse/components/commonComponent/cm_select_component/types';
import writeXlsxFile from 'write-excel-file';
import moment from 'moment';

import { Overlay, OverlayRef } from '@angular/cdk/overlay';

import _ from 'lodash';

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
    selector: 'sys_item_index',
    templateUrl: './index.component.html',
    styleUrls: ['./index.component.scss']
})

export class sys_item_indexComponent extends BaseIndexDatatableComponent {
    listSysItemType: { id: string; name: string }[] = [];
    filter: {search:string,status_del:string,id_item_type:string}
    public listStatusDel: CmSelectType[];
    listMapTree: { [key: string]: TreeNodeInterface[] } = {};

    constructor(http: HttpClient, dialog: MatDialog
        , _translocoService: TranslocoService
        , fuseConfirmationService: FuseConfirmationService
        , route: ActivatedRoute
        , @Inject('BASE_URL') baseUrl: string,
         _overlay: Overlay, _viewContainerRef: ViewContainerRef
    ) {
        super(http, baseUrl, _translocoService, fuseConfirmationService, route, dialog, 'sys_item',
            { search: "", status_del: "1", id_item_type: "-1" },_overlay,_viewContainerRef
        )

        this.listStatusDel = [
            {
                id: "1",
                name: this._translocoService.translate('system.use')
            },
            {
                id: "2",
                name: this._translocoService.translate('system.not_use')
            }
        ];

        this.getListItemType();

    }
    getListItemType() {
        this.http
            .post('/sys_item_type.ctr/getListUse/', {}
            ).subscribe((resp: { id: string; name: string }[]) => {
                this.listSysItemType = resp;
                this.listSysItemType.splice(0, 0, { id: "-1", name: this._translocoService.translate('all') })
            });
    }

    openDialogAdd(): void {
        const dialogRef = this.dialog.open(sys_item_popUpAddComponent, {
            disableClose: true,
            width: '90%',
            data: {
                actionEnum: 1,
                db: {
                    id: 0,
                    status_del: 1
                },
                list_sys_item_unit_other: [],
                list_item_min_max_stock: [],
                list_item_quality : [],
            },
        });
        dialogRef.afterClosed().subscribe(result => {
            if (result.db.id == 0) return;
            this.rerender();
        });
    }
    revertStatus(model, pos): void {
        model.db.status_del = 1;
        this.http
            .post(this.controller + '.ctr/edit/',
                {
                    data: model,
                }
            ).subscribe(resp => {
                this.rerender();
            },
                error => {
                    console.log(error);

                });
        this.rerender();
    }
    openDialogEdit(model, pos): void {
        model.actionEnum = actionEnums.Edit;
        const dialogRef = this.dialog.open(sys_item_popUpAddComponent, {
            disableClose: true,
            data: model
        });
        dialogRef.afterClosed().subscribe(result => {
            if (result != undefined && result != null) {
                this.rerender();
            }

        });
    }
    openDialogDetail(model, pos): void {
        model.actionEnum = actionEnums.Detail;
        const dialogRef = this.dialog.open(sys_item_popUpAddComponent, {
            disableClose: true,
            data: model
        });
        dialogRef.afterClosed().subscribe(result => {
            if (result != undefined && result != null) this.listData[pos] = result;
        });
    }



    ngOnInit(): void {
        //this.baseInitData(this.pageIndex,this.pageSize,null,null,[]);
    }
    generateExcel() {
        if (this.listData.length <= 0) {
            this.showMessageWarningNoConfirm('no_data');
            return;
        }

        const schema = [
            {
                column: 'Loại mặt hàng',
                type: String,
                value: row => row.sys_item_type_name,
                width: 20,
            },
            {
                column: 'Mã',
                type: String,
                value: row => row.db.code_item,
                width: 20,
            },
            {
                column: 'Tên',
                type: String,
                value: row => row.db.name,
                width: 20,
            },
            {
                column: 'Đơn vị tính',
                type: String,
                value: row => row.sys_unit_name,
                width: 20,
            },
            {
                column: 'Loại',
                type: String,
                value: row => row.db.type == 1 ? "Sản phẩm" : row.db.type == 2 ? "Bán thành phẩm" : "Vật tư",
                width: 20,
            }, {
                column: 'Loại chi phí',
                type: String,
                value: row => row.sys_cost_type_name,
                width: 20,
            }, {
                column: 'Quy cách',
                type: Number,
                value: row => row.db.has_specification,
                width: 20,
            }, {
                column: 'Cấu hình B.O.M',
                type: Number,
                value: row => row.db.has_bom,
                width: 20,
            }, {
                column: 'Người lập',
                type: String,
                value: row => row.createby_name,
                width: 20,
            }, {
                column: 'Ngày lập',
                type: String,
                value: row => moment(row.db.create_date).format('YYYY/MM/DD'),
                width: 20,
            }, {
                column: 'Ghi chú',
                type: String,
                value: row => row.db.note,
                width: 20,
            },
        ]
        writeXlsxFile(this.listData, {
            schema,
            headerStyle: {
                backgroundColor: '#eeeeee',
                fontWeight: 'bold',
                align: 'center'
            },
            fileName: this._translocoService.translate('NAV.sys_item')
        })
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
         // console.log(node)
        if (!hashMap[node.key]) {
          hashMap[node.key] = true;
          array.push(node);
        }
      }

      handleDataBefor() {

        this.listData.map(e => {
          e.type = e.db.type;
          e.key = e.db.id;
          return e;
        });

        var groups = _.groupBy(this.listData, 'type');


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



    cleanFilter() {
        this.filter.status_del = "1";
        this.filter.id_item_type = "-1";
    }
}


