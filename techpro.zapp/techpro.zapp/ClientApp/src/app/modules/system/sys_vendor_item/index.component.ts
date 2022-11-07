import { Component, Inject, ViewChild } from '@angular/core';


import { HttpClient, HttpResponse } from '@angular/common/http';

import { DataTablesResponse } from 'app/Basecomponent/datatable';

import { TranslocoService } from '@ngneat/transloco';
import { sys_vendor_item_popUpAddComponent } from './popupAdd.component';
import { MatDialog } from '@angular/material/dialog';

import { Subject } from 'rxjs';
import { BaseIndexDatatableComponent } from 'app/Basecomponent/BaseIndexDatatable.component';

import { FuseConfirmationService } from '@fuse/services/confirmation';
import { ActivatedRoute } from '@angular/router';
import { actionEnums } from 'app/Basecomponent/actionEnums';
import writeXlsxFile from 'write-excel-file';
import moment from 'moment';
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
    selector: 'sys_vendor_item_index',
    templateUrl: './index.component.html',
    styleUrls: ['./index.component.scss']
})

export class sys_vendor_item_indexComponent extends BaseIndexDatatableComponent
{
    listCustomerName: any;
    item_chose_factory: any;
    public listposition: any;
    listSupplier: any;
    listMapTree: { [key: string]: TreeNodeInterface[] } = {};
    constructor(http: HttpClient, dialog: MatDialog
        , _translocoService: TranslocoService
        , fuseConfirmationService: FuseConfirmationService,route: ActivatedRoute
        , @Inject('BASE_URL') baseUrl: string
        ) {
        super(http, baseUrl, _translocoService, fuseConfirmationService, route, dialog,'sys_vendor_item',
            { search: "", id_supplier: "-1"}
        )
        this.getListSuplliers();

    }
    getListSuplliers(): void {
        this.http.post('/sys_customer.ctr/getListUseCustomner/', { search: '' }).subscribe((resp) => {
            this.listSupplier = resp;
            this.listSupplier.splice(0, 0, { id: "-1", name: this._translocoService.translate('all') });
        });
    }
    openDialogAdd(): void {
        if (this.filter.id_supplier == "") {
            this.showMessageWarningNoConfirm("system.must_chose_supplier")
            return
        }

        const dialogRef = this.dialog.open(sys_vendor_item_popUpAddComponent, {
            disableClose: true,
            data: {
                actionEnum: 1,
                db: {
                    id_supplier: this.filter.id_supplier,

                    id: 0,
                }
            },
        });
        dialogRef.afterClosed().subscribe(result => {
            if (result.db.id == 0) return;
            this.rerender();
        });
    }
    openDialogEdit(model, pos): void {

     model.actionEnum = actionEnums.Edit;
        const dialogRef = this.dialog.open(sys_vendor_item_popUpAddComponent, {
            disableClose: true,
            data: model
        });
        dialogRef.afterClosed().subscribe(result => {
            if (result != undefined && result!=null) this.listData[pos] = result;
        });
    }
    openDialogDetail(model, pos): void {
         model.actionEnum = actionEnums.Detail;
        const dialogRef = this.dialog.open(sys_vendor_item_popUpAddComponent, {
            disableClose: true,
            data: model
        });
        dialogRef.afterClosed().subscribe(result => {
            if (result != undefined && result != null) this.listData[pos] = result;
        });
    }
    openDialogHistory(): void {
        if (this.filter.id_supplier == "") {
            this.showMessageWarningNoConfirm("system.must_chose_supplier")
            return
        }

        let model = {
            actionEnum: 5,
            db: {
                id_supplier: this.filter.id_supplier,
                id: 0,
            }};
        const dialogRef = this.dialog.open(sys_vendor_item_popUpAddComponent, {
            disableClose: true,
           data: model
        });

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
          e.sys_item_name = e.sys_item_name;
          e.key = e.db.id;
          return e;
        });

        var groups = _.groupBy(this.listData, 'sys_item_name');


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
        console.log(this.listData)
      }


    ngOnInit(): void {
        //this.baseInitData(this.pageIndex,this.pageSize,null,null,[]);
    }
    // generateExcel() {
    //     if (this.listData.length <= 0) {
    //         this.showMessageWarningNoConfirm('no_data');
    //         return;
    //     }
    //     const schema = [
    //         {
    //             column: 'Nhà cung cấp',
    //             type: String,
    //             value: row => row.supplier_name,
    //             width: 20,
    //         },
    //         {
    //             column: 'Mặt hàng',
    //             type: String,
    //             value: row => row.sys_item_name,
    //             width: 20,
    //         },
    //         {
    //             column: 'Quy cách',
    //             type: String,
    //             value: row => row.sys_item_specification_name,
    //             width: 20,
    //         },
    //         {
    //             column: 'Đơn vị tính',
    //             type: String,
    //             value: row => row.sys_unit_name,
    //             width: 20,
    //         },{
    //             column: 'Giá tiền (VNĐ)',
    //             type: Number,
    //             value: row => row.db?.price_item,
    //             width: 20,
    //         },{
    //             column: 'Số lượng đặt tối thiểu',
    //             type: Number,
    //             value: row => row.db?.min_stock_order,
    //             width: 20,
    //         },{
    //             column: 'Thời gian hàng về ( ngày )',
    //             type: Number,
    //             value: row => row.db?.delivery_time,
    //             width: 20,
    //         },
    //         {
    //             column: 'Ghi chú',
    //             type: String,
    //             value: row => row.db?.note,
    //             width: 20,
    //         }
    //     ]
    //     writeXlsxFile(this.listData, {
    //         schema,
    //         headerStyle: {
    //             backgroundColor: '#eeeeee',
    //             fontWeight: 'bold',
    //             align: 'center'
    //         },
    //         fileName: this._translocoService.translate('NAV.sys_vendor_item')
    //     })
    // }

}


