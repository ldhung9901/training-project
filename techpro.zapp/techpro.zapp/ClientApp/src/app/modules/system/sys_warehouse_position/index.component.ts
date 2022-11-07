import { Component, Inject, ViewChild } from '@angular/core';


import { HttpClient, HttpResponse } from '@angular/common/http';

import { DataTablesResponse } from 'app/Basecomponent/datatable';

import { TranslocoService } from '@ngneat/transloco';
import { sys_warehouse_position_popUpAddComponent } from './popupAdd.component';
import { MatDialog } from '@angular/material/dialog';

import { Subject } from 'rxjs';
import { BaseIndexDatatableComponent } from 'app/Basecomponent/BaseIndexDatatable.component';

import { FuseConfirmationService } from '@fuse/services/confirmation';
import { ActivatedRoute } from '@angular/router';
import { actionEnums } from 'app/Basecomponent/actionEnums';
import { CmSelectType } from '@fuse/components/commonComponent/cm_select_component/types';
import writeXlsxFile from 'write-excel-file';
import moment from 'moment';

@Component({
    selector: 'sys_warehouse_position_index',
    templateUrl: './index.component.html',
    styleUrls: ['./index.component.scss']
})

export class sys_warehouse_position_indexComponent extends BaseIndexDatatableComponent
{
    listwarehouse: any;
    public listStatusDel: CmSelectType[];

    constructor(http: HttpClient, dialog: MatDialog
        , _translocoService: TranslocoService
        , fuseConfirmationService: FuseConfirmationService,route: ActivatedRoute
        , @Inject('BASE_URL') baseUrl: string
        ) {
        super(http, baseUrl, _translocoService, fuseConfirmationService, route, dialog,'sys_warehouse_position',
            { search: "", id_warehouse: "", status_del: "1" }
        )
        this.http
            .post('/sys_warehouse.ctr/getListUse/', {}
            ).subscribe(resp => {
                this.listwarehouse = resp;
            });

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
    }
    openDialogAdd(): void {
        const dialogRef = this.dialog.open(sys_warehouse_position_popUpAddComponent, {
            disableClose: true,
            data: {
                actionEnum: 1,
                db: {
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
        const dialogRef = this.dialog.open(sys_warehouse_position_popUpAddComponent, {
            disableClose: true,
            data: model
        });
        dialogRef.afterClosed().subscribe(result => {
            if (result != undefined && result!=null) this.listData[pos] = result;
        });
    }
    openDialogDetail(model, pos): void {
         model.actionEnum = actionEnums.Detail;
        const dialogRef = this.dialog.open(sys_warehouse_position_popUpAddComponent, {
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
                value: row => row.warehouse_name,
                width: 20,
            },
            {
                column: 'Tên vị trí kho',
                type: String,
                value: row => row.db.name,
                width: 20,
            },
            {
                column: 'Hàng',
                type: Number,
                value: row => row.db.row,
                width: 20,
            },
            {
                column: 'Cột',
                type: Number,
                value: row => row.db.col,
                width: 20,
            },
            {
                column: 'Người lập',
                type: String,
                value: row => row.createby_name,
                width: 20,
            },
            {
                column: 'Ngày lập',
                type: String,
                value: row => moment(row.db.create_date).format('DD/MM/YYYY'),
                width: 20,
            },
            {
                column: 'Ghi chú',
                type: String,
                value: row => row.db.note,
                width: 20,
            }

        ]
        writeXlsxFile(this.listData, {
            schema,
            headerStyle: {
                backgroundColor: '#eeeeee',
                fontWeight: 'bold',
                align: 'center'
            },
            fileName: this._translocoService.translate('NAV.sys_warehouse_position')
        })
    }
}


