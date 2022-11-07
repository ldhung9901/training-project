import { Component, Inject, ViewChild } from '@angular/core';


import { HttpClient, HttpResponse } from '@angular/common/http';

import { DataTablesResponse } from 'app/Basecomponent/datatable';

import { TranslocoService } from '@ngneat/transloco';
import { sys_workstation_template_opc_popUpAddComponent } from './popupAdd.component';
import { MatDialog } from '@angular/material/dialog';

import { Subject } from 'rxjs';
import { BaseIndexDatatableComponent } from 'app/Basecomponent/BaseIndexDatatable.component';

import { FuseConfirmationService } from '@fuse/services/confirmation';
import { ActivatedRoute } from '@angular/router';
import { actionEnums } from 'app/Basecomponent/actionEnums';

@Component({
    selector: 'sys_workstation_template_opc_index',
    templateUrl: './index.component.html',
    styleUrls: ['./index.component.scss']
})

export class sys_workstation_template_opc_indexComponent extends BaseIndexDatatableComponent
{
    item_chose: any;
    list_sys_workstation: any;
    constructor(http: HttpClient, dialog: MatDialog
        , _translocoService: TranslocoService
        , @Inject('BASE_URL') baseUrl: string
        , fuseConfirmationService: FuseConfirmationService, route: ActivatedRoute
        ) {
        super(http, baseUrl, _translocoService, fuseConfirmationService, route, dialog,'sys_workstation_template_opc',
            { search: "", id_workstation:""}
        )

        this.http.post('/sys_workstation.ctr/getListUse', {}
        ).subscribe(resp => {
            this.list_sys_workstation = resp;
        });

    }
    bind_data_item(): void {
        this.rerender();
    }
    openDialogAdd(): void {
        if (this.filter.id_workstation == "" || this.filter.id_workstation == undefined) {

            this.showMessageWarningNoConfirm("system.must_chose_workstation")
            return;
        }
        const dialogRef = this.dialog.open(sys_workstation_template_opc_popUpAddComponent, {
            disableClose: true,
            data: {
                actionEnum: 1,
                db: {
                    id: 0,
                    id_workstation: this.item_chose.id,
                },
                workstation_name: this.item_chose.name
            },
        });
        dialogRef.afterClosed().subscribe(result => {
            if (result.db.id == 0) return;
            this.rerender();
        });
    }
    openDialogEdit(model, pos): void {
     model.actionEnum = actionEnums.Edit;
        const dialogRef = this.dialog.open(sys_workstation_template_opc_popUpAddComponent, {
            disableClose: true,
            data: model
        });
        dialogRef.afterClosed().subscribe(result => {
            if (result != undefined && result!=null) this.listData[pos] = result;
        });
    }
    openDialogDetail(model, pos): void {
     model.actionEnum = actionEnums.Detail;
        const dialogRef = this.dialog.open(sys_workstation_template_opc_popUpAddComponent, {
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


