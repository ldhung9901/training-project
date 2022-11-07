import { Component, Inject, ViewChild } from '@angular/core';


import { HttpClient, HttpResponse } from '@angular/common/http';

import { DataTablesResponse } from 'app/Basecomponent/datatable';
import { TranslocoService } from '@ngneat/transloco';
import { sys_help_popUpAddComponent } from './popupAdd.component';
import { MatDialog } from '@angular/material/dialog';

import { Subject } from 'rxjs';
import { BaseIndexDatatableComponent } from 'app/Basecomponent/BaseIndexDatatable.component';

import { FuseConfirmationService } from '@fuse/services/confirmation';
import { ActivatedRoute } from '@angular/router';
import { CmSelectType } from '@fuse/components/commonComponent/cm_select_component/types';


type NewType = CmSelectType;

@Component({
    selector: 'sys_help_index',
    templateUrl: './index.component.html',
    styleUrls: ['./index.component.scss']
})

export class sys_help_indexComponent extends BaseIndexDatatableComponent
{
    public listStatusDel: NewType[];


    constructor(http: HttpClient, dialog: MatDialog
        , _translocoService: TranslocoService
        , fuseConfirmationService: FuseConfirmationService,route: ActivatedRoute
        , @Inject('BASE_URL') baseUrl: string
        ) {
        super(http, baseUrl, _translocoService, fuseConfirmationService, route, dialog,'sys_help',
            { search: "", status_del: "1" }
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
        const dialogRef = this.dialog.open(sys_help_popUpAddComponent, {
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
        const dialogRef = this.dialog.open(sys_help_popUpAddComponent, {
            disableClose: true,

            data: model
        });
        dialogRef.afterClosed().subscribe(result => {
            if (result != undefined && result!=null) this.listData[pos] = result;
        });
    }
    openDialogDetail(model, pos): void {
    model.actionEnum = 3;
        const dialogRef = this.dialog.open(sys_help_popUpAddComponent, {
            disableClose: true,

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


