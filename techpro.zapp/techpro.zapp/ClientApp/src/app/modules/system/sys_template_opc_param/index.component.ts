import { Component, Inject, ViewChild } from '@angular/core';


import { HttpClient, HttpResponse } from '@angular/common/http';

import { DataTablesResponse } from 'app/Basecomponent/datatable';

import { TranslocoService } from '@ngneat/transloco';
import { sys_template_opc_param_popUpAddComponent } from './popupAdd.component';
import { MatDialog } from '@angular/material/dialog';

import { Subject } from 'rxjs';
import { BaseIndexDatatableComponent } from 'app/Basecomponent/BaseIndexDatatable.component';

import { FuseConfirmationService } from '@fuse/services/confirmation';
import { ActivatedRoute } from '@angular/router';
import { actionEnums } from 'app/Basecomponent/actionEnums';
import { CmSelectType } from '@fuse/components/commonComponent/cm_select_component/types';

@Component({
    selector: 'sys_template_opc_param_index',
    templateUrl: './index.component.html',
    styleUrls: ['./index.component.scss']
})

export class sys_template_opc_param_indexComponent extends BaseIndexDatatableComponent
{
    item_chose: any;
    list_template_opc: any;
    public listStatusDel: CmSelectType[];

    constructor(http: HttpClient, dialog: MatDialog
        , _translocoService: TranslocoService
        , @Inject('BASE_URL') baseUrl: string
        , fuseConfirmationService: FuseConfirmationService, route: ActivatedRoute
        ) {
        super(http, baseUrl, _translocoService, fuseConfirmationService, route, dialog,'sys_template_opc_param',
            { search: "", id_template_opc: "", status_del: "1" }
        )

        this.http.post('/sys_template_opc.ctr/getListUse', {}
        ).subscribe(resp => {
            this.list_template_opc = resp;
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
    bind_data_item(): void {
        this.rerender();
    }
    openDialogAdd(): void {
        if (this.filter.id_template_opc == "" || this.filter.id_template_opc == undefined) {

            this.showMessageWarningNoConfirm("system.must_chose_template_opc")
            return;
        }
        const dialogRef = this.dialog.open(sys_template_opc_param_popUpAddComponent, {
            disableClose: true,
            data: {
                actionEnum: 1,
                db: {
                    id: 0,
                    id_template_opc: this.item_chose.id,
                    chart_type:1,
                },
                template_opc_name: this.item_chose.name
            },
        });
        dialogRef.afterClosed().subscribe(result => {
            if (result.db.id == 0) return;
            this.rerender();
        });
    }
    openDialogEdit(model, pos): void {
     model.actionEnum = actionEnums.Edit;
        const dialogRef = this.dialog.open(sys_template_opc_param_popUpAddComponent, {
            disableClose: true,
            data: model
        });
        dialogRef.afterClosed().subscribe(result => {
            if (result != undefined && result!=null) this.listData[pos] = result;
        });
    }
    openDialogDetail(model, pos): void {
     model.actionEnum = actionEnums.Detail;
        const dialogRef = this.dialog.open(sys_template_opc_param_popUpAddComponent, {
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


}


