import { Component, Inject, ViewChild, ViewContainerRef } from '@angular/core';


import { HttpClient, HttpResponse } from '@angular/common/http';

import { DataTablesResponse } from 'app/Basecomponent/datatable';
import { TranslocoService } from '@ngneat/transloco';
import { sys_version_popUpAddComponent } from './popupAdd.component';
import { MatDialog } from '@angular/material/dialog';

import { Subject } from 'rxjs';
import { BaseIndexDatatableComponent } from 'app/Basecomponent/BaseIndexDatatable.component';

import { FuseConfirmationService } from '@fuse/services/confirmation';
import { ActivatedRoute } from '@angular/router';
import moment from 'moment';
import { Overlay } from '@angular/cdk/overlay';


@Component({
    selector: 'sys_version_index',
    templateUrl: './index.component.html',
    styleUrls: ['./index.component.scss']
})

export class sys_version_indexComponent extends BaseIndexDatatableComponent {
    // public listStatusDel: CmSelectType[];
    public list_help: any = [];
    optionDateSelectedId: number;
    disabledOptionDate: boolean;

    constructor(http: HttpClient, dialog: MatDialog
        , _translocoService: TranslocoService
        , fuseConfirmationService: FuseConfirmationService, route: ActivatedRoute
        , @Inject('BASE_URL') baseUrl: string, _overlay: Overlay, _viewContainerRef: ViewContainerRef
    ) {
        super(http, baseUrl, _translocoService, fuseConfirmationService, route, dialog, 'sys_version',
            {
                search: "", range: {
                    type: 5,
                    start: moment().startOf('month').toDate(),
                    end: moment().endOf('month').toDate(),
                }
            }, _overlay, _viewContainerRef
        )

        // this.listStatusDel = [
        //     {
        //         id: "1",
        //         name: this._translocoService.translate('system.use')
        //     },
        //     {
        //         id: "2",
        //         name: this._translocoService.translate('system.not_use')
        //     }
        // ];
    }
    openDialogAdd(): void {
        const dialogRef = this.dialog.open(sys_version_popUpAddComponent, {
            disableClose: true,
            panelClass: 'my-full-screen-dialog',
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
        const dialogRef = this.dialog.open(sys_version_popUpAddComponent, {
            disableClose: true,
            panelClass: 'my-full-screen-dialog',
            data: model
        });
        dialogRef.afterClosed().subscribe(result => {
            if (result != undefined && result != null) this.listData[pos] = result;
        });
    }
    openDialogDetail(model, pos): void {
        model.actionEnum = 3;
        const dialogRef = this.dialog.open(sys_version_popUpAddComponent, {
            disableClose: true,
            panelClass: 'my-full-screen-dialog',
            data: model
        });
        dialogRef.afterClosed().subscribe(result => {
            if (result != undefined && result != null) this.listData[pos] = result;
        });
    }

    bindingSelectOptionDate(type: 'select' | 'datepicker'): void {
        if (type === 'datepicker') {
            this.filter.range.type = [...this.listSelectOptionDate].find(e =>

                (moment(this.filter.range.start).format('DD-MM-YYYY') === moment(e.value.start).format('DD-MM-YYYY')) &&
                (moment(this.filter.range.end).format('DD-MM-YYYY') === moment(e.value.end).format('DD-MM-YYYY'))

            )?.id || [...this.listSelectOptionDate][0].id;
        }
        else if (type === 'select') {
            const findedItem = this.listSelectOptionDate.find(e => e.id == this.filter.range.type);
            this.filter.range = { ...findedItem.value, type: findedItem.id };
        }
        this.rerender();
    }

    ngOnInit(): void {
        // this.baseInitData();
    }
    cleanFilter() {
        this.filter = {
            search: "",
            range: {
                type: 5,
                start: moment().startOf('month').toDate(),
                end: moment().endOf('month').toDate(),
            }
        };

        this.optionDateSelectedId = 0;
        this.disabledOptionDate = false;
    }

}


