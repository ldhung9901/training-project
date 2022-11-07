import { Component, Inject, ViewChild } from '@angular/core';


import { HttpClient, HttpResponse } from '@angular/common/http';

import { DataTablesResponse } from 'app/Basecomponent/datatable';

import { TranslocoService } from '@ngneat/transloco';
import { sys_factory_line_item_capacity_popUpAddComponent } from './popupAdd.component';
import { MatDialog } from '@angular/material/dialog';

import { Subject } from 'rxjs';
import { BaseIndexDatatableComponent } from 'app/Basecomponent/BaseIndexDatatable.component';

import { FuseConfirmationService } from '@fuse/services/confirmation';
import { ActivatedRoute } from '@angular/router';
import { actionEnums } from 'app/Basecomponent/actionEnums';

@Component({
    selector: 'sys_factory_line_item_capacity_index',
    templateUrl: './index.component.html',
    styleUrls: ['./index.component.scss']
})

export class sys_factory_line_item_capacity_indexComponent extends BaseIndexDatatableComponent
{
    listfactory: any;
    item_chose_factory: any;
    public listfactoryline: any;

    constructor(http: HttpClient, dialog: MatDialog
        , _translocoService: TranslocoService
        , fuseConfirmationService: FuseConfirmationService,route: ActivatedRoute
        , @Inject('BASE_URL') baseUrl: string
        ) {
        super(http, baseUrl, _translocoService, fuseConfirmationService, route, dialog,'sys_factory_line_item_capacity',
            { search: "", id_sys_factory_line: "", id_sys_factory: ""}
        )
        this.http
            .post('/sys_factory.ctr/getListUse/', {}
            ).subscribe(resp => {
                this.listfactory = resp;
            });

    }
    bind_data_factory(): void {
        this.filter.id_sys_factory_line = "";
        this.rerender();
        this.http
            .post('/sys_factory_line.ctr/getListUse/', {
                id_factory: this.filter.id_sys_factory
            }
            ).subscribe(resp => {
                this.listfactoryline = resp;
            });
    }
    openDialogAdd(): void {
        if (this.filter.id_sys_factory == "") {
            this.showMessageWarningNoConfirm("system.must_chose_factory")
            return
        }
        if (this.filter.id_sys_factory_line == "") {
            this.showMessageWarningNoConfirm("system.must_chose_factory_line")
            return
        }
        const dialogRef = this.dialog.open(sys_factory_line_item_capacity_popUpAddComponent, {
            disableClose: true,
            data: {
                actionEnum: 1,
                db: {
                    id_sys_factory_line: this.filter.id_sys_factory_line,
                    id_sys_factory: this.filter.id_sys_factory,
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
        const dialogRef = this.dialog.open(sys_factory_line_item_capacity_popUpAddComponent, {
            disableClose: true,
            data: model
        });
        dialogRef.afterClosed().subscribe(result => {
            if (result != undefined && result!=null) this.listData[pos] = result;
        });
    }
    openDialogDetail(model, pos): void {
         model.actionEnum = actionEnums.Detail;
        const dialogRef = this.dialog.open(sys_factory_line_item_capacity_popUpAddComponent, {
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


}


