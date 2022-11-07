import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';


import { HttpClient } from '@angular/common/http';

import { TranslocoService } from '@ngneat/transloco';
import { BasePopUpAddComponent } from 'app/Basecomponent/BasePopupAdd.component';
import { FuseConfirmationService } from '@fuse/services/confirmation';
import { ActivatedRoute } from '@angular/router';



@Component({
    selector: 'sys_customer_popupAdd',
    templateUrl: 'popupAdd.html',
    styleUrls: ['./popupAdd.component.scss']
})
export class sys_customer_popUpAddComponent extends BasePopUpAddComponent {

    constructor(public dialogRef: MatDialogRef<sys_customer_popUpAddComponent>,
        http: HttpClient, _translocoService: TranslocoService,
        fuseConfirmationService: FuseConfirmationService, route: ActivatedRoute,
        @Inject('BASE_URL') baseUrl: string,
        @Inject(MAT_DIALOG_DATA) data: any) {
        super(_translocoService, fuseConfirmationService, route,baseUrl,http,'sys_customer', dialogRef);
        this.record = data;
        this.Oldrecord = JSON.parse(JSON.stringify(data));
        this.actionEnum = data.actionEnum;
        if (this.actionEnum == 1) {
            this.baseInitData();
        }
    }
    beforesave():boolean{
        if ((this.record.db.is_customer??true) ===false && (this.record.db.is_supplier??true) ===false) {
            this.showMessageWarningNoConfirm('must_choose_customer_or_supplier')
            return false;
        }
        return true;
    }


}
