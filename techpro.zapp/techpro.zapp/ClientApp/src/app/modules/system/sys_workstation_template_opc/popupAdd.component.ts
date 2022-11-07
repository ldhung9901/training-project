import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';


import { HttpClient } from '@angular/common/http';
import { BasePopUpAddComponent } from 'app/Basecomponent/BasePopupAdd.component';
import { TranslocoService } from '@ngneat/transloco';
import { FuseConfirmationService } from '@fuse/services/confirmation';
import { ActivatedRoute } from '@angular/router';


@Component({
    selector: 'sys_workstation_template_opc_popupAdd',
    templateUrl: 'popupAdd.html',
})
export class sys_workstation_template_opc_popUpAddComponent extends BasePopUpAddComponent {

    listsys_template_opc: any;
    constructor(public dialogRef: MatDialogRef<sys_workstation_template_opc_popUpAddComponent>,
        http: HttpClient, _translocoService: TranslocoService,
        fuseConfirmationService: FuseConfirmationService, route: ActivatedRoute,
        @Inject('BASE_URL') baseUrl: string,
        @Inject(MAT_DIALOG_DATA) data: any) {
        super(_translocoService, fuseConfirmationService, route,baseUrl,http,'sys_workstation_template_opc', dialogRef);
        this.record = data;
        this.Oldrecord = JSON.parse(JSON.stringify(data));
        this.actionEnum = data.actionEnum;
        if (this.actionEnum != 3) {
            this.http
                .post('/sys_template_opc.ctr/getListUse/', {}
                ).subscribe(resp => {
                    this.listsys_template_opc = resp;
                });


        }
        if (this.actionEnum == 1) {
            this.baseInitData();
        }
    }

}
