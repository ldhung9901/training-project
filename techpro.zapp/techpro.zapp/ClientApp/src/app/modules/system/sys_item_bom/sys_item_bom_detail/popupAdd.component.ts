import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';

import { HttpClient } from '@angular/common/http';
import { BasePopUpAddComponent } from 'app/Basecomponent/BasePopupAdd.component';
import { TranslocoService } from '@ngneat/transloco';
import { FuseConfirmationService } from '@fuse/services/confirmation';
import { ActivatedRoute } from '@angular/router';
import { sys_item_bom_config_model } from '../type';



@Component({
    selector: 'sys_item_bom_detail_popUpAdd',
    templateUrl: 'popUpAdd.html',
})
export class sys_item_bom_detail_popUpAddComponent extends BasePopUpAddComponent {



    listType: { id: number, name: string }[] = [];
    record: sys_item_bom_config_model;

    public list_type_evaluate: any = [{ id: "boolean", name: this._translocoService.translate("quality.yes_or_no") }, { id: "number", name: this._translocoService.translate("quality.measure") }];
    item_chose_specification: any;
    item_bom_chose: any;
    dataFilter_specification: { id_item: string; };
    dataFilter: { ignoreIds: string; };

    constructor(public dialogRef: MatDialogRef<any>,
        http: HttpClient, _translocoService: TranslocoService,
        fuseConfirmationService: FuseConfirmationService, route: ActivatedRoute,
        @Inject('BASE_URL') baseUrl: string,
        @Inject(MAT_DIALOG_DATA) data: sys_item_bom_config_model) {
        super(_translocoService, fuseConfirmationService, route, baseUrl, http, 'sys_item_bom', dialogRef);
        this.record = { ...data };
        this.actionEnum = data.actionEnum;
        if (this.actionEnum == 1) {
            this.resetAddItem();
        }

    }

    resetAddItem(): void {
        // this.record = {
        //     db: {

        //         id_item: '',
        //         config_code: '',
        //         id_specification: 0,
        //         id_unit: '',
        //         note: '',
        //         name: '',
        //         status_use: false,
        //         aplly_date: '',
        //     },
        // };
    }
    changeStatus(event: { checked: boolean }) {
        if (event.checked) {
            this.record.db.status_use = true;
        }
        if (!event.checked) {
            this.record.db.status_use = false;
        }
    }
    bind_data_item_bomchose() {
        console.log(this.item_bom_chose);

        this.record.db.id_item = this.item_bom_chose.id;
        this.record.db.id_unit = this.item_bom_chose.id_unit;
        this.dataFilter_specification = {
            id_item: this.record.db.id_item
        }

    }
    bind_data_item_specification_chose() {
        console.log(this.item_chose_specification);
        this.record.db.id_unit = this.item_chose_specification.id_unit;
        this.record.specification_name = this.item_chose_specification.name;
    }
    save_config(): void {
        if (this.actionEnum === 1) {
            this.http
                .post<sys_item_bom_config_model>('sys_item_bom.ctr/create_config/',
                    {
                        data: this.record,
                    }
                ).subscribe(resp => {
                    this.record = resp;

                    this.basedialogRef.close(this.record);


                },
                    error => {
                        if (error.status == 400) {
                            this.errorModel = error.error;

                        }
                        this.loading = false;

                    }
                );
        } else if (this.actionEnum === 2) {
            this.http
                .post('sys_item_bom.ctr/edit_config/',
                    {
                        data: this.record,
                    }
                ).subscribe((resp) => {
                    this.basedialogRef.close(this.record);
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
                })
            this.actionEnum = 3;
        }
    }
    close(): void {
        this.basedialogRef.close(this.record);
        this.actionEnum = 3;
    }
}
