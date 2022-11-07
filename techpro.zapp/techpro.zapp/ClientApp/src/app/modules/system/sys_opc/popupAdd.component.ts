import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';


import { HttpClient } from '@angular/common/http';
import { BasePopUpAddComponent } from 'app/Basecomponent/BasePopupAdd.component';
import { TranslocoService } from '@ngneat/transloco';
import { FuseConfirmationService } from '@fuse/services/confirmation';
import { ActivatedRoute } from '@angular/router';


@Component({
    selector: 'sys_opc_popupAdd',
    templateUrl: 'popupAdd.html',
})
export class sys_opc_popUpAddComponent extends BasePopUpAddComponent {
    public listsys_unit: any;
    public list_opc_client: any;

    constructor(public dialogRef: MatDialogRef<sys_opc_popUpAddComponent>,
        http: HttpClient, _translocoService: TranslocoService,
        fuseConfirmationService: FuseConfirmationService, route: ActivatedRoute,
        @Inject('BASE_URL') baseUrl: string,
        @Inject(MAT_DIALOG_DATA) data: any) {
        super(_translocoService, fuseConfirmationService, route,baseUrl,http,'sys_opc', dialogRef);
        this.record = data;
        this.Oldrecord = JSON.parse(JSON.stringify(data));
        this.actionEnum = data.actionEnum;
        this.http
            .post('/sys_unit.ctr/getListUse/', {}
            ).subscribe(resp => {
                this.listsys_unit = resp;


            });
        this.http.post('/sys_opc_client.ctr/getListUse', {}
        ).subscribe(resp => {
            this.list_opc_client = resp;
        });
        if (this.actionEnum == 1) {
            this.baseInitData();
        }
    }

}
