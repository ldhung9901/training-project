import { Component, Inject, ViewChild } from '@angular/core';


import { HttpClient, HttpResponse } from '@angular/common/http';

import { DataTablesResponse } from 'app/Basecomponent/datatable';

import { TranslocoService } from '@ngneat/transloco';
import { sys_opc_popUpAddComponent } from './popupAdd.component';
import { MatDialog } from '@angular/material/dialog';

import { Subject } from 'rxjs';
import { BaseIndexDatatableComponent } from 'app/Basecomponent/BaseIndexDatatable.component';

import { FuseConfirmationService } from '@fuse/services/confirmation';
import { ActivatedRoute } from '@angular/router';
import { actionEnums } from 'app/Basecomponent/actionEnums';
import { CmSelectType } from '@fuse/components/commonComponent/cm_select_component/types';

@Component({
    selector: 'sys_opc_index',
    templateUrl: './index.component.html',
    styleUrls: ['./index.component.scss']
})

export class sys_opc_indexComponent extends BaseIndexDatatableComponent
{
    public list_opc_client: any;
    public listStatusDel: CmSelectType[];

    constructor(http: HttpClient, dialog: MatDialog
        , _translocoService: TranslocoService
        , @Inject('BASE_URL') baseUrl: string
        , fuseConfirmationService: FuseConfirmationService, route: ActivatedRoute
        ) {
        super(http, baseUrl, _translocoService, fuseConfirmationService, route, dialog,'sys_opc',
            { search: "", id_opcclient: "", status_del: "1" }
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
        this.http.post('/sys_opc_client.ctr/getListUse', {}
        ).subscribe(resp => {
            this.list_opc_client = resp;
        });
    }
    openDialogAdd(): void {
        const dialogRef = this.dialog.open(sys_opc_popUpAddComponent, {
            disableClose: true,
            data: {
                actionEnum: 1,
                db: {
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
        const dialogRef = this.dialog.open(sys_opc_popUpAddComponent, {
            disableClose: true,
            data: model
        });
        dialogRef.afterClosed().subscribe(result => {
            if (result != undefined && result!=null) this.listData[pos] = result;
        });
    }
    openDialogDetail(model, pos): void {
     model.actionEnum = actionEnums.Detail;
        const dialogRef = this.dialog.open(sys_opc_popUpAddComponent, {
            disableClose: true,
            data: model
        });
        dialogRef.afterClosed().subscribe(result => {
            if (result != undefined && result!=null) this.listData[pos] = result;
        });
    }
    testConnect(model, pos): void {
        this.http
            .post(this.controller + '.ctr/conectopc/',
                {
                    id: model.db.id,
                }
        ).subscribe(resp => {
            this.showMessageSuccessNoConfirm("Gi?? tr??? hi???n t???i: "+resp);
            },
                error => {
                }
            );
    }


    ngOnInit(): void {
        //this.baseInitData(this.pageIndex,this.pageSize,null,null,[]);
    }


}


