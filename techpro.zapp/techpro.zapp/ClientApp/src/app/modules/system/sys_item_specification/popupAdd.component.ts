import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';


import { HttpClient } from '@angular/common/http';
import { BasePopUpAddComponent } from 'app/Basecomponent/BasePopupAdd.component';
import { TranslocoService } from '@ngneat/transloco';
import { FuseConfirmationService } from '@fuse/services/confirmation';
import { ActivatedRoute } from '@angular/router';



@Component({
    selector: 'sys_item_specification_popupAdd',
    templateUrl: 'popupAdd.html',
})
export class sys_item_specification_popUpAddComponent extends BasePopUpAddComponent {


    public listsys_unit: any;

    constructor(public dialogRef: MatDialogRef<sys_item_specification_popUpAddComponent>,
        http: HttpClient, _translocoService: TranslocoService,
        fuseConfirmationService: FuseConfirmationService, route: ActivatedRoute,
        @Inject('BASE_URL') baseUrl: string,
        @Inject(MAT_DIALOG_DATA) data: any) {
        super(_translocoService, fuseConfirmationService, route,baseUrl,http,'sys_item_specification', dialogRef);
        this.record = data;
        this.Oldrecord = JSON.parse(JSON.stringify(data));
        this.actionEnum = data.actionEnum;
        this.http
            .post('/sys_unit.ctr/getListUse/', {}
            ).subscribe(resp => {
                this.listsys_unit = resp;
            });
        if (this.actionEnum == 1) {
            this.baseInitData();
        }
    }




}
