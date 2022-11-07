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



@Component({
    selector: 'sys_warehouse_position_popupAdd',
    templateUrl: 'popupAdd.html',
})
export class sys_warehouse_position_popUpAddComponent extends BasePopUpAddComponent  {

    listwarehouse: any;
    searchwarehouse: String = '';

    protected _onDestroy = new Subject<void>();
    /** control for the selected bank */

    constructor(public dialogRef: MatDialogRef<sys_warehouse_position_popUpAddComponent>,
        http: HttpClient, _translocoService: TranslocoService,
        fuseConfirmationService: FuseConfirmationService, route: ActivatedRoute,
        @Inject('BASE_URL') baseUrl: string,
        @Inject(MAT_DIALOG_DATA) data: any) {
        super(_translocoService, fuseConfirmationService, route,baseUrl,http,'sys_warehouse_position', dialogRef);
        this.record = data;
        this.Oldrecord = JSON.parse(JSON.stringify(data));
        this.http
            .post('/sys_warehouse.ctr/getListUse/', {}
            ).subscribe(resp => {
                this.listwarehouse = resp;
            });
        this.actionEnum = data.actionEnum;
        if (this.actionEnum == 1) {
            this.baseInitData();
        }
    }

}
