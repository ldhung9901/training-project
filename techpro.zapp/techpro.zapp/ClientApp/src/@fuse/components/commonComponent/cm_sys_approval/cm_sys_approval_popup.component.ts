import { Component, OnInit, ViewEncapsulation, Input, EventEmitter, Output, OnDestroy, Inject } from '@angular/core';
import { FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { TranslocoService } from '@ngneat/transloco';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ActivatedRoute } from '@angular/router';
import { BasePopUpAddComponent } from 'app/Basecomponent/BasePopupAdd.component';
import { FuseConfirmationService } from '@fuse/services/confirmation';
import { translateDataTable } from 'app/Basecomponent/VietNameseDataTable';

@Component({
    selector     : 'cm_sys_approval_popup',
    templateUrl  : './cm_sys_approval_popup.component.html'
})
export class cm_sys_approval_popupComponent extends BasePopUpAddComponent
{
    public listData_sys_approval:any
    public item_sys_approval_config: any = {
        list_item: []
    };


    constructor(public dialogRef: MatDialogRef<cm_sys_approval_popupComponent>,
        http: HttpClient, _translocoService: TranslocoService,
        fuseConfirmationService: FuseConfirmationService, route: ActivatedRoute,
        @Inject('BASE_URL') baseUrl: string,
        @Inject(MAT_DIALOG_DATA) data: any) {
        super(_translocoService,fuseConfirmationService, route,baseUrl, http,'sys_approval',dialogRef);

        this.record = data;;
        this.record.db.id_sys_approval_config = null;
        this.actionEnum = data.actionEnum;
        this.http
            .post('/sys_approval_config.ctr/getListUse/',
                {
                    data: {
                        menu: this.record.db.menu
                    }
                }
            ).subscribe(resp => {
                this.listData_sys_approval = resp;
                this.loading = false;
            });

    }
    ngOnInit(): void {
        this.loading = false;
    }
    bind_sys_approval_config(): void {
        this.record.db.menu = this.item_sys_approval_config.db.menu;
        this.record.db.total_step = this.item_sys_approval_config.db.step;
        this.record.list_step = this.item_sys_approval_config.list_item;

    }

}

