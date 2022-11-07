import { Component, Inject, Input, ViewChild } from '@angular/core';


import { HttpClient, HttpResponse } from '@angular/common/http';

import { DataTablesResponse } from 'app/Basecomponent/datatable';

import { TranslocoService } from '@ngneat/transloco';

import { MatDialog } from '@angular/material/dialog';

import { Subject } from 'rxjs';
import { BaseIndexDatatableComponent } from 'app/Basecomponent/BaseIndexDatatable.component';

import { FuseConfirmationService } from '@fuse/services/confirmation';
import { ActivatedRoute } from '@angular/router';

import { actionEnums } from 'app/Basecomponent/actionEnums';
import { MatTabChangeEvent } from '@angular/material/tabs';
import { sys_item_quality } from 'app/modules/system/sys_item/types';
import { sys_item_bom_config_model, sys_item_bom_model, sys_item_bom_quality_db, sys_item_bom_quality_model } from '../../type';



@Component({
    selector: 'sys_item_bom_detail_quality',
    templateUrl: './index.component.html',
    styleUrls: ['./index.component.scss']
})

export class sys_item_bom_detail_quality_indexComponent extends BaseIndexDatatableComponent {
    item_chose: any;
    errorModel: number;
    public item_chose_config: any;
    tabIndex: number = 0;
    actionEnum: number;
    listPhase: { id: string, name: string }[];
    public list_type_evaluate: any = [{ id: "boolean", name: this._translocoService.translate("quality.yes_or_no") }, { id: "number", name: this._translocoService.translate("quality.measure") }];
    @Input() data: sys_item_bom_config_model;
    item_chose_phase: any;
    record: sys_item_bom_config_model;

    addItem: sys_item_bom_quality_model;
    list_item_quality: sys_item_bom_quality_model[];
    list_bom_config: sys_item_bom_config_model;
    constructor(http: HttpClient, dialog: MatDialog
        , _translocoService: TranslocoService
        , fuseConfirmationService: FuseConfirmationService, route: ActivatedRoute
        , @Inject('BASE_URL') baseUrl: string
    ) {
        super(http, baseUrl, _translocoService, fuseConfirmationService, route, dialog, 'sys_item_bom',
            { search: "", id_item: "", id_item_bom_config: 0 }
        )

        this.resetAddItem();
        this.getListPhase();

    }
    getListPhase(): void {
        this.http
            .post<{ id: string, name: string }[]>('/sys_phase.ctr/getListUse/', {}
            ).subscribe(resp => {
                this.listPhase = resp;
            });

    }
    getListQuality(id: string, id_item_bom_config:number){
        this.http.get<sys_item_bom_quality_model[]>('sys_item_bom.ctr/get_list_quality/', {
            params:{
                id_item:id,
                id_item_bom_config:id_item_bom_config,
            }
        }).subscribe(resp => {

            this.list_item_quality = resp

        })
    }
    addItemIntoDB(model): void {
        this.http
            .post(this.controller + '.ctr/create_quality/',
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
                this.getListQuality(this.data.db.id_item, this.data.db.id)
            });

    }

    ngOnChanges() {
        this.actionEnum = this.data.actionEnum;
        this.list_item_quality = this.data.list_item_quality


    }
    resetAddItem(): void {
        this.addItem = {
            db: {
                id_item: '',
                id_item_bom_config: 0,
                id_phase: '',
                content: '',
                description: '',
                type_evaluate: '',
            }
        }

    }
    // public rerender(): void {
    //     this.http.get<sys_item_bom_model>('sys_item_bom.ctr/getAll/', {}).subscribe(resp => {

    //         this.list_bom_config = resp

    //     })
    // }
    addDetail(): void {
        this.addItem.db.id_item_bom_config = this.data.db.id;
        this.addItem.db.id_item = this.data.db.id_item;

        let filter_content = this.list_item_quality.filter(e => e.db.content == this.addItem.db.content);
        if (!this.addItem.db.content) {
            this.showMessageWarningNoConfirm("system.must_have_content")
            return
        }
        if (filter_content.length > 0) {
            this.showMessageWarningNoConfirm("system.content_existed")
            return
        }
        if (!this.addItem.db.id_phase) {
            this.showMessageWarningNoConfirm("system.must_have_phase")
            return
        }
        if (!this.addItem.db.type_evaluate) {
            this.showMessageWarningNoConfirm("system.must_have_type_evaluate")
            return
        }


        this.addItemIntoDB(this.addItem);
        this.list_item_quality = [...this.list_item_quality, this.addItem]

        this.resetAddItem();

    }
    tabChanged(tabChangeEvent: MatTabChangeEvent): void {
        this.tabIndex = tabChangeEvent.index;
    }

    trackByFn(index, item): any {


        return item.id || index;
    }
    bindDataPhaseChoose(): void {
        console.log(this.item_chose_phase);


        this.addItem.db.id_phase = this.item_chose_phase.id;
        this.addItem.phase_name = this.item_chose_phase.name;
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
                    .post(this.controller + '.ctr/delete_quality/',
                        {
                            id: id,
                        }
                    ).subscribe(resp => {

                        this.list_item_quality.splice(i, 1);
                        this.list_item_quality = [...this.list_item_quality];
                    });
            }
        });

    }
}


