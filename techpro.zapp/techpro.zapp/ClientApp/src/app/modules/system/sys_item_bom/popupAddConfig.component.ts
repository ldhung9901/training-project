import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';


import { HttpClient } from '@angular/common/http';
import { BasePopUpAddComponent } from 'app/Basecomponent/BasePopupAdd.component';
import { TranslocoService } from '@ngneat/transloco';
import { FuseConfirmationService } from '@fuse/services/confirmation';
import { ActivatedRoute } from '@angular/router';



@Component({
    selector: 'sys_item_bom_popupAdd_config',
    templateUrl: 'popupAddConfig.html',
})
export class sys_item_bom_popupAdd_configComponent extends BasePopUpAddComponent {

    public item_chose_specification: any;
    public listData_specification: any;
    public dataFilter_specification: any;

    item_bom_chose: any;
    list_phase: any;
    list_specification: any;
    public dataFilter: any;
    constructor(public dialogRef: MatDialogRef<sys_item_bom_popupAdd_configComponent>,
        http: HttpClient, _translocoService: TranslocoService,
        fuseConfirmationService: FuseConfirmationService, route: ActivatedRoute,
        @Inject('BASE_URL') baseUrl: string,
        @Inject(MAT_DIALOG_DATA) data: any) {
        super(_translocoService, fuseConfirmationService, route,baseUrl,http,'sys_item_bom', dialogRef);
        this.record = data;
        this.Oldrecord = JSON.parse(JSON.stringify(data));
        this.dataFilter = {
             ignoreIds:this.record.db.id_item
        }
        this.dataFilter_specification = {
            id_item: this.record.db.id_item
        }
        this.actionEnum = data.actionEnum;
        if (this.actionEnum == 1) {
            this.baseInitData();
        }
    }


    bind_data_item_specification_chose(): void {
        this.record.db.id_unit = this.item_chose_specification.id_unit;
        this.record.db.sys_unit_name = this.item_chose_specification.name;
    }
    save_config(): void {
            this.http
                .post(this.controller + '.ctr/create_config/',
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
    }


}
