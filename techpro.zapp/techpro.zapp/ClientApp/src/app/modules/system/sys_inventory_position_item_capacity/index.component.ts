import { Component, Inject, ViewChild } from '@angular/core';


import { HttpClient, HttpResponse } from '@angular/common/http';

import { DataTablesResponse } from 'app/Basecomponent/datatable';

import { TranslocoService } from '@ngneat/transloco';
import { sys_inventory_position_item_capacity_popUpAddComponent } from './popupAdd.component';
import { MatDialog } from '@angular/material/dialog';

import { Subject } from 'rxjs';
import { BaseIndexDatatableComponent } from 'app/Basecomponent/BaseIndexDatatable.component';

import { FuseConfirmationService } from '@fuse/services/confirmation';
import { ActivatedRoute } from '@angular/router';
import { actionEnums } from 'app/Basecomponent/actionEnums';
import writeXlsxFile from 'write-excel-file';
import moment from 'moment';


@Component({
    selector: 'sys_inventory_position_item_capacity_index',
    templateUrl: './index.component.html',
    styleUrls: ['./index.component.scss']
})

export class sys_inventory_position_item_capacity_indexComponent extends BaseIndexDatatableComponent
{
    listwarehouse: any;
    item_chose_factory: any;
    public listposition: any;

    constructor(http: HttpClient, dialog: MatDialog
        , _translocoService: TranslocoService
        , fuseConfirmationService: FuseConfirmationService,route: ActivatedRoute
        , @Inject('BASE_URL') baseUrl: string
        ) {
        super(http, baseUrl, _translocoService, fuseConfirmationService, route, dialog,'sys_inventory_position_item_capacity',
            { search: "", id_warehouse: "", id_position: ""}
        )
        this.http
            .post('/sys_warehouse.ctr/getListUse/', {}
            ).subscribe(resp => {
                this.listwarehouse = resp;
            });

    }
    bind_data_warehouse(): void {
        this.filter.id_position = "";
        this.rerender();
        this.http
            .post('/sys_warehouse_position.ctr/getListUse/', {
                id_warehouse: this.filter.id_warehouse
            }
            ).subscribe(resp => {
                this.listposition = resp;
            });
    }
    openDialogAdd(): void {
        if (this.filter.id_warehouse == "") {
            this.showMessageWarningNoConfirm("system.must_chose_warehouse")
            return
        }
        if (this.filter.id_position == "") {
            this.showMessageWarningNoConfirm("system.must_chose_position")
            return
        }
        const dialogRef = this.dialog.open(sys_inventory_position_item_capacity_popUpAddComponent, {
            disableClose: true,
            data: {
                actionEnum: 1,
                db: {
                    id_position: this.filter.id_position,
                    id_warehouse: this.filter.id_warehouse,
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
        const dialogRef = this.dialog.open(sys_inventory_position_item_capacity_popUpAddComponent, {
            disableClose: true,
            data: model
        });
        dialogRef.afterClosed().subscribe(result => {
            if (result != undefined && result!=null) this.listData[pos] = result;
        });
    }
    openDialogDetail(model, pos): void {
         model.actionEnum = actionEnums.Detail;
        const dialogRef = this.dialog.open(sys_inventory_position_item_capacity_popUpAddComponent, {
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
                column: 'Kho',
                type: String,
                value: row => row.inventory_name,
                width: 20,
            },
            {
                column: 'Vị trí',
                type: String,
                value: row => row.position_name,
                width: 20,
            },
            {
                column: 'Mặt hàng',
                type: String,
                value: row => row.sys_item_name,
                width: 20,
            },
            {
                column: 'Quy cách',
                type: String,
                value: row => row.sys_item_specification_name,
                width: 20,
            },
            {
                column: 'Đơn vị tính',
                type: String,
                value: row => row.sys_unit_name,
                width: 20,
            },{
                column: 'Tồn kho tối thiểu',
                type: Number,
                value: row => row.db.min_stock,
                width: 20,
            },{
                column: 'Tồn kho tối đa	',
                type: Number,
                value: row => row.db.max_stock,
                width: 20,
            },{
                column: 'Người lập',
                type: String,
                value: row => row.createby_name,
                width: 20,
            },{
                column: 'Ngày lập',
                type: String,
                value: row => moment(row.db.create_date).format('YYYY/MM/DD'),
                width: 20,
            },{
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
            fileName: this._translocoService.translate('NAV.sys_inventory_position_item_capacity')
        })
    }


}


