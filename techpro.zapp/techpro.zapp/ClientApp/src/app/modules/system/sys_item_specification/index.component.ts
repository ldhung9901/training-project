import { Component, Inject, ViewChild } from '@angular/core';


import { HttpClient, HttpResponse } from '@angular/common/http';

import { DataTablesResponse } from 'app/Basecomponent/datatable';

import { TranslocoService } from '@ngneat/transloco';
import { sys_item_specification_popUpAddComponent } from './popupAdd.component';
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
    selector: 'sys_item_specification_index',
    templateUrl: './index.component.html',
    styleUrls: ['./index.component.scss']
})

export class sys_item_specification_indexComponent extends BaseIndexDatatableComponent
{
    public listStatusDel: CmSelectType[];

    item_chose: any;
    constructor(http: HttpClient, dialog: MatDialog
        , _translocoService: TranslocoService
        , fuseConfirmationService: FuseConfirmationService, route: ActivatedRoute
        , @Inject('BASE_URL') baseUrl: string
        ) {
        super(http, baseUrl, _translocoService, fuseConfirmationService, route, dialog,'sys_item_specification',
            { search: "", id_item: "", status_del: "1" }
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
    }
    openDialogAdd(): void {
        if (this.filter.id_item == "" || this.filter.id_item == undefined) {
            this.showMessageWarningNoConfirm('system.must_chose_item')

            return;
        }
        const dialogRef = this.dialog.open(sys_item_specification_popUpAddComponent, {
            disableClose: true,
            data: {
                actionEnum:1,
                db: {
                    id: 0,
                    id_item: this.item_chose.id,
                    id_unit: this.item_chose.id_unit
                },
                sys_item_name: this.item_chose.name,
                sys_item_unit_name: this.item_chose.unit_name,
            },
        });
        dialogRef.afterClosed().subscribe(result => {
            if (result.db.id == 0) return;
            this.rerender();
        });
    }
    openDialogEdit(model, pos): void {
     model.actionEnum = actionEnums.Edit;
        const dialogRef = this.dialog.open(sys_item_specification_popUpAddComponent, {
            disableClose: true,
            data: model
        });
        dialogRef.afterClosed().subscribe(result => {
            if (result != undefined && result!=null) this.listData[pos] = result;
        });
    }
    openDialogDetail(model, pos): void {
     model.actionEnum = actionEnums.Detail;
        const dialogRef = this.dialog.open(sys_item_specification_popUpAddComponent, {
            disableClose: true,
            data: model
        });
        dialogRef.afterClosed().subscribe(result => {
            if (result != undefined && result!=null) this.listData[pos] = result;
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
                column: 'M???t h??ng',
                type: String,
                value: row => row.sys_item_name,
                width: 20,
            },
            {
                column: 'T??n',
                type: String,
                value: row => row.db.name,
                width: 20,
            },
            {
                column: 'Ng??y l???p',
                type: String,
                value: row => moment(row.db.create_date).format('YYYY/MM/DD'),
                width: 20,
            },
            {
                column: 'Ng?????i l???p',
                type: String,
                value: row => row.createby_name,
                width: 20,
            },
            {
                column: 'Ghi ch??',
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
            fileName: this._translocoService.translate('NAV.sys_item_specification')
        })
    }


}


