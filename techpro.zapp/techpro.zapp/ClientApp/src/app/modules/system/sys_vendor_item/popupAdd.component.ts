import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';


import { HttpClient } from '@angular/common/http';
import { BasePopUpAddComponent } from 'app/Basecomponent/BasePopupAdd.component';
import { TranslocoService } from '@ngneat/transloco';
import { FormControl } from '@angular/forms';
import { ReplaySubject, Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { FuseConfirmationService } from '@fuse/services/confirmation';
import { ActivatedRoute } from '@angular/router';

import { BaseIndexDatatableComponent } from 'app/Basecomponent/BaseIndexDatatable.component';
import { DataTablesResponse } from 'app/Basecomponent/datatable';



@Component({
    selector: 'sys_vendor_item_popupAdd',
    templateUrl: 'popupAdd.html',
})
export class sys_vendor_item_popUpAddComponent extends BasePopUpAddComponent  {

    listfactory: any;
    searchfactory: String = '';

    protected _onDestroy = new Subject<void>();
    /** control for the selected bank */
    public item_chose_specification: any;
    public listData_specification: any;
    public dataFilter_specification: any;
    public item_chose: any;
    public list_specification: any;
    public listData: any;
    public currentIndex: number;
    public listSupplier: any;
    constructor(public dialogRef: MatDialogRef<sys_vendor_item_popUpAddComponent>,
        http: HttpClient, _translocoService: TranslocoService,
        fuseConfirmationService: FuseConfirmationService, route: ActivatedRoute,
        @Inject('BASE_URL') baseUrl: string,
        @Inject(MAT_DIALOG_DATA) data: any) {
        super(_translocoService, fuseConfirmationService, route, baseUrl, http, 'sys_vendor_item', dialogRef);
        this.record = data;
        this.Oldrecord = JSON.parse(JSON.stringify(data));
        this.actionEnum = data.actionEnum;

        if (this.actionEnum == 1) {
            this.baseInitData();
        }
        if (this.actionEnum == 5) {


            this.baseInitDataHistory(this.record);

        }
        this.dataFilter_specification = {
            id_item: ""
        }
        this.getListSuplliers();

    }

    getListSuplliers(): void {
        this.http.post('/sys_customer.ctr/getListUseCustomner/', { search: '' }).subscribe((resp) => {
            this.listSupplier = resp;
        });
    }

    baseInitDataHistory(record: any) {
        const that = this;

    }
    bind_data_itemchose(): void {
        this.record.db.id_unit = this.item_chose.id_unit;
        this.record.sys_unit_name = this.item_chose.unit_name;
        this.dataFilter_specification = {
            id_item: this.record.db.id_item
        }
    }
    bind_data_item_specification_chose(): void {
        this.record.db.id_unit = this.item_chose_specification.id_unit;
        this.record.sys_item_specification_name = this.item_chose_specification.name;
        this.record.sys_unit_name = this.item_chose_specification.sys_unit_name;
    }

}
