import { Component, Inject, ViewChild } from '@angular/core';


import { HttpClient, HttpResponse } from '@angular/common/http';

import { DataTablesResponse } from 'app/Basecomponent/datatable';
import { TranslocoService } from '@ngneat/transloco';
import { sys_help_detail_popUpAddComponent } from './popupAdd.component';
import { MatDialog } from '@angular/material/dialog';

import { Subject } from 'rxjs';
import { BaseIndexDatatableComponent } from 'app/Basecomponent/BaseIndexDatatable.component';

import { FuseConfirmationService } from '@fuse/services/confirmation';
import { ActivatedRoute } from '@angular/router';
import { CmSelectType } from '@fuse/components/commonComponent/cm_select_component/types';


@Component({
    selector: 'sys_help_detail_index',
    templateUrl: './index.component.html',
    styleUrls: ['./index.component.scss']
})

export class sys_help_detail_indexComponent extends BaseIndexDatatableComponent
{
    public listStatusDel: CmSelectType[];
    public list_help: any = [];

    constructor(http: HttpClient, dialog: MatDialog
        , _translocoService: TranslocoService
        , fuseConfirmationService: FuseConfirmationService,route: ActivatedRoute
        , @Inject('BASE_URL') baseUrl: string
        ) {
        super(http, baseUrl, _translocoService, fuseConfirmationService, route, dialog,'sys_help_detail',
            { search: "", status_del: "1",id_help:"" }
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
        this.http.post("/sys_help.ctr/getListUse/", {}).subscribe((resp) => {
            this.list_help = resp;
        });
    }
    openDialogAdd(): void {
        if (this.filter.id_help === "" || this.filter.id_help === null) {
           this.showMessageWarningNoConfirm('system.must_chose_help')
            return;
        }
        const dialogRef = this.dialog.open(sys_help_detail_popUpAddComponent, {
            disableClose: true,
            panelClass: 'my-full-screen-dialog',
            data: {
                actionEnum: 1,
                db: {
                    id: 0,
                    id_help: this.filter.id_help
                }
            },
        });
        dialogRef.afterClosed().subscribe(result => {
            if (result.db.id == 0) return;
            this.rerender();
        });
    }
    revertStatus(id): void {
        this.http
            .post(this.controller + '.ctr/revert/',
                {
                    id: id,
                }
            ).subscribe(resp => {
                this.rerender();
            },
                error => {
                    // console.log(error);

                });
        this.rerender();
    }
    openDialogEdit(model, pos): void {
    model.actionEnum = 2;
        const dialogRef = this.dialog.open(sys_help_detail_popUpAddComponent, {
            disableClose: true,
            panelClass: 'my-full-screen-dialog',
            data: model
        });
        dialogRef.afterClosed().subscribe(result => {
            if (result != undefined && result!=null) this.listData[pos] = result;
        });
    }
    openDialogDetail(model, pos): void {
    model.actionEnum = 3;
        const dialogRef = this.dialog.open(sys_help_detail_popUpAddComponent, {
            disableClose: true,
            panelClass: 'my-full-screen-dialog',
            data: model
        });
        dialogRef.afterClosed().subscribe(result => {
            if (result != undefined && result!=null) this.listData[pos] = result;
        });
    }



    ngOnInit(): void {
        // this.baseInitData();
    }


}


