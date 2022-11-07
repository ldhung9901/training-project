import { Component, EventEmitter, Inject, Input, Output, } from '@angular/core';


import { HttpClient } from '@angular/common/http';



import { TranslocoService } from '@ngneat/transloco';

import { MatDialog } from '@angular/material/dialog';

import { BaseIndexDatatableComponent } from 'app/Basecomponent/BaseIndexDatatable.component';

import { FuseConfirmationService } from '@fuse/services/confirmation';
import { ActivatedRoute } from '@angular/router';

import { actionEnums } from 'app/Basecomponent/actionEnums';
import { MatTabChangeEvent } from '@angular/material/tabs';
import { sys_item_bom_config_model, sys_item_bom_model } from '../type';
import { sys_item_bom_detail_popUpAddComponent } from './popupAdd.component';


@Component({
    selector: 'sys_item_bom_detail',
    templateUrl: './index.component.html',
    styleUrls: ['./index.component.scss']
})

export class sys_item_bom_detail_indexComponent extends BaseIndexDatatableComponent {
    item_chose: any;
    public item_chose_config: any;
    tabIndex: number = 0;
    actionEnum: number;
    addItem: any = [];
    @Output() messageDelete = new EventEmitter<string>();
    @Input() data: sys_item_bom_config_model;
    listPhase: { id: string, name: string }[];
    item_bom_chose: any;
    item_chose_specification: any;
    dataFilter: { ignoreIds: any; };
    dataFilter_specification: { id_item: any; };
    item_chose_phase: any;
    basedialogRef: any;
    record: sys_item_bom_model;
    Oldrecord: sys_item_bom_model[];
    list_item_quota: sys_item_bom_model[];

    constructor(http: HttpClient, dialog: MatDialog
        , _translocoService: TranslocoService
        , fuseConfirmationService: FuseConfirmationService, route: ActivatedRoute
        , @Inject('BASE_URL') baseUrl: string
    ) {
        super(http, baseUrl, _translocoService, fuseConfirmationService, route, dialog, 'sys_item_bom',
            { search: "", id_item: "", id_item_bom_config: 0 }
        )



    }

    resetAddItem(): void {
        this.record = {
            db: {

                id_specification: 0,
                id_phase: "",
                note: "",
                quota: 0,
            },
            sys_item_bom_name: "",

        };
    }

    rerender(): void {
        this.http
            .get<sys_item_bom_model[]>('/sys_item_bom.ctr/get_item_quota/', {
                params : { id_item: this.data.db.id_item, id_item_bom_config: this.data.db.id }
            }
            ).subscribe(resp => {
                this.list_item_quota = resp;
            });
    }

    addDetail(): void {


        this.Oldrecord = this.listData;
        this.record.db.id_item_bom_config = this.data.db.id;
        this.record.db.id_item = this.data.db.id_item;

        // Cùng mặt hàng trên một cấu hình == false
        let filter_item = this.list_item_quota.filter(e => e.db.id_item_bom == this.record.db.id_item_bom && e.db.id_item_bom_config == this.record.db.id_item_bom_config);
        if (filter_item.length > 0) {
            this.showMessageWarningNoConfirm('system.item_existed');
            return
        }

        this.list_item_quota = [...this.list_item_quota, this.record]
        this.addItemIntoDB(this.record);
        this.resetAddItem();

    }
    ngOnChanges() {


        this.filter.id_item_bom_config = this.data.db.id;
        this.filter.id_item = this.data.db.id_item;

        this.list_item_quota = this.data.list_item_quota;

        this.actionEnum = this.data.actionEnum;
        if (this.actionEnum = 2) {
            this.getListPhase();
        }
        this.resetAddItem();

    }

    tabChanged(tabChangeEvent: MatTabChangeEvent): void {
        this.tabIndex = tabChangeEvent.index;
    }

    getListPhase(): void {
        this.http
            .post<{ id: string, name: string }[]>('/sys_phase.ctr/getListUse/', {}
            ).subscribe(resp => {
                this.listPhase = resp;
            });

    }
    trackByFn(index, item): any {


        return item.id || index;
    }
    openDialogEdit(model: sys_item_bom_config_model): void {

        model.actionEnum = actionEnums.Edit;
        const dialogRef = this.dialog.open(sys_item_bom_detail_popUpAddComponent, {
            disableClose: true,
            data: model
        });
        dialogRef.afterClosed().subscribe(result => {
            // if (result != undefined && result != null) this.listData[pos] = result;
        });

    }

    deleteDetailOld(id: string, i: number): void {
        this.fuseConfirmationService.open({
            title: 'areYouSure',
            message: '',
            icon: {
                name: 'heroicons_outline:trash',
                color: 'error'
            }
        }).afterClosed().subscribe((result) => {


            // If the confirm button pressed...
            if (result === 'confirmed') {
                this.http
                    .post(this.controller + '.ctr/delete/',
                        {
                            id: id,
                        }
                    ).subscribe(resp => {
                        // this.rerender();
                        this.list_item_quota.splice(i, 1);
                        this.list_item_quota = [...this.list_item_quota];
                    });
            }
        });

    }


    bind_data_item_bomchose(): void {

        this.record.db.id_unit = this.item_bom_chose.id_unit;
        this.record.sys_item_bom_name = this.item_bom_chose.name;
        this.record.sys_item_code = this.item_bom_chose.code_item;
        this.dataFilter_specification = {
            id_item: this.record.db.id_item_bom
        }
    }
    bind_data_item_specification_chose(): void {
        this.record.db.id_unit = this.item_chose_specification.id_unit;
        this.record.specification_name = this.item_chose_specification.name;
    }
    bind_data_phase_choose(): void {
        this.record.db.id_unit = this.item_chose_phase.id_unit;
        this.record.phase_name = this.item_chose_phase.name;
    }

    addItemIntoDB(model): void {
        this.http
            .post(this.controller + '.ctr/create/',
                {
                    data: model
                }
            ).subscribe((resp) => {
                this.fuseConfirmationService.open({
                    title: 'save_success',
                    message: '',
                    actions: {
                        confirm: { show: false }
                    },
                    icon: {
                        name: 'heroicons_solid:check',
                        color: 'success'
                    }
                })
                this.rerender();
            });

    }

    changeTimeQuota(model, newData) {
        console.log(model, newData);

        if (newData == null || newData < 0) {
            this.showMessageWarningNoConfirm('dps.printer_uptime_not_valid');
            return;
        } else if (newData != model.db.printer_uptime) {
            model.db.quota = newData;

            this.http.post(this.controller + '.ctr/edit/',
                {
                    data: model,
                }).subscribe((resp) => {

                });
        }
    }
}


