import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';

import { HttpClient } from '@angular/common/http';
import { BasePopUpAddComponent } from 'app/Basecomponent/BasePopupAdd.component';
import { TranslocoService } from '@ngneat/transloco';
import { FuseConfirmationService } from '@fuse/services/confirmation';
import { ActivatedRoute } from '@angular/router';

import { sys_item_min_max_stock_model, sys_item_model, sys_item_quality, sys_item_quality_detail_db, sys_item_unit_other_model } from './types';
import { isThisSecond } from 'date-fns';


@Component({
    selector: 'sys_item_popUpAddDetail',
    templateUrl: 'popUpAddDetail.html',
})
export class sys_item_popUpAddDetailComponent extends BasePopUpAddComponent {



    listType: { id: number, name: string }[] = [];
    addItem: sys_item_quality_detail_db = {

    };
    record: sys_item_quality

    public list_type_evaluate: any = [{ id: "boolean", name: this._translocoService.translate("quality.yes_or_no") }, { id: "number", name: this._translocoService.translate("quality.measure") }];
    isChecked: boolean;

    constructor(public dialogRef: MatDialogRef<sys_item_popUpAddDetailComponent>,
        http: HttpClient, _translocoService: TranslocoService,
        fuseConfirmationService: FuseConfirmationService, route: ActivatedRoute,
        @Inject('BASE_URL') baseUrl: string,
        @Inject(MAT_DIALOG_DATA) data: sys_item_quality) {
        super(_translocoService, fuseConfirmationService, route, baseUrl, http, 'sys_item', dialogRef);

        this.record = { ...data };

        this.actionEnum = data.actionEnum;
        this.resetAddItem();
        this.isChecked = this.record.db.status_use;


    }
    getDetail(): void {

    }
    resetAddItem(): void {
        this.addItem = {

            config_content: '',
            description: '',
            type_evaluate: '',
            number_evaluate: null,
            error_range_start: null,
            error_range_end: null,
        };
    }
    addDetail(): void {
        var valid = true;
        var error = '';

        if (this.record.list_item_quality.filter(d => d.config_content == this.addItem.config_content).length > 0) {
            error += this._translocoService.translate('system.content_exist');
            valid = false;
        }
        if (!this.addItem.type_evaluate) {
            error += this._translocoService.translate('system.must_have_type_evaluate');
            valid = false;
        }
        if (!valid) {
            this.showMessageWarningNoConfirm(error);
            return;
        }

        this.record.list_item_quality.splice(0, 0, this.addItem);
        this.record.list_item_quality = [...this.record.list_item_quality]


        this.resetAddItem();
    }
    trackByFn(index: number, item: any): any {
        return item.id || index;
    }
    checkValid(): boolean {
        let error = ''
        let valid = true;
        if (this.record.list_item_quality.length == 0) {
            error = 'not_valid';
            valid = false;

        }
        if (this.record.db.config_code == null || this.record.db.config_code == undefined) {
            error = 'system.must_have_config_code';
            valid = false;

        }
        if (this.record.db.aplly_date == null || this.record.db.aplly_date == undefined) {
            error = 'system.must_have_apply_date';
            valid = false;

        }if (this.record.db.config_name == null || this.record.db.config_name == undefined) {
            error = 'system.must_have_config_name';
            valid = false;

        }


        if (!valid) {
            this.showMessageWarningNoConfirm(error)
        }

        return valid

    }
    save(): void {
        if (!this.checkValid()) {
            return
        }
        this.dialogRef.close(this.record);
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
        //list data

        // this.http
        //     .post(this.controller + '.ctr/create_item/',
        //         {
        //             data: this.record,
        //         }
        //     ).subscribe((resp) => {
        //         this.fuseConfirmationService.open({
        //             title: 'save_success',
        //             message: '',
        //             actions: {
        //                 confirm: { show: false }
        //             },
        //             icon: {
        //                 name: 'heroicons_solid:check',
        //                 color: 'success'
        //             }
        //         })
        //     });
    }
    deleteDetailOld(i: number): void {
        this.record.list_item_quality.splice(i, 1);
        this.record.list_item_quality = [...this.record.list_item_quality];
    }
    changeStatus(event: { checked: boolean }) {
        console.log(event.checked);
        if (event.checked) {
            this.isChecked = true;
            this.record.db.status_use = true;
        }
        if (!event.checked) {
            this.isChecked = false;
            this.record.db.status_use = false;
        }
    }
}
