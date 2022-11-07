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
    selector: 'sys_factory_line_popupAdd',
    templateUrl: 'popupAdd.html',
})
export class sys_factory_line_popUpAddComponent extends BasePopUpAddComponent {

    public additem: any;
    listfactory: any;
    searchfactory: String = '';
    item_chose: any;
    protected _onDestroy = new Subject<void>();
    /** control for the selected bank */

    constructor(public dialogRef: MatDialogRef<sys_factory_line_popUpAddComponent>,
        http: HttpClient, _translocoService: TranslocoService,
        fuseConfirmationService: FuseConfirmationService, route: ActivatedRoute,
        @Inject('BASE_URL') baseUrl: string,
        @Inject(MAT_DIALOG_DATA) data: any) {
        super(_translocoService, fuseConfirmationService, route, baseUrl, http, 'sys_factory_line', dialogRef);
        this.record = data;
        this.Oldrecord = JSON.parse(JSON.stringify(data));
        this.http
            .post('/sys_factory.ctr/getListUse/', {}
            ).subscribe(resp => {
                this.listfactory = resp;
            });
        this.actionEnum = data.actionEnum;
        this.resetAddItem();

        if (this.actionEnum == 1) {
            this.baseInitData();
        }
    }


    addDetail(): void {
        var valid = true;
        var error = '';

        if (this.record.list_maintenance_system.filter(d => d.name == this.additem.name).length > 0) {
            error += this._translocoService.translate('existed');
            valid = false;
        }


        if (!valid) {
            this.showMessageWarningNoConfirm(error);
            return;
        }
        this.record.list_maintenance_system = [...this.record.list_maintenance_system,this.additem]


        this.resetAddItem();
    }
    deleteDetail(pos): void {


        this.record.list_maintenance_system.splice(pos, 1);
        this.record.list_maintenance_system = [... this.record.list_maintenance_system]
    }
    resetAddItem(): void {

        this.additem = {
            id: 0,
            id_factory: this.record.db.id ?? null,
            id_system: null,
            note: "",
            name: "",
        };

    }
    bind_data_item_chose(): void {
        this.additem = {
            maintenance_system_name: this.item_chose.name,
            db: {
                id: 0,
                id_system: this.item_chose.id,

            }
        }
    }

}
