import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';


import { HttpClient } from '@angular/common/http';
import { BasePopUpAddComponent } from 'app/Basecomponent/BasePopupAdd.component';
import { TranslocoService } from '@ngneat/transloco';
import { FuseConfirmationService } from '@fuse/services/confirmation';
import { ActivatedRoute } from '@angular/router';



@Component({
    selector: 'sys_approval_config_popupAdd',
    templateUrl: 'popupAdd.html',
})
export class sys_approval_config_popUpAddComponent extends BasePopUpAddComponent {
    public additem: any;
    public item_chose: any;

    public listMenu: any;
    constructor(public dialogRef: MatDialogRef<sys_approval_config_popUpAddComponent>,
        http: HttpClient, _translocoService: TranslocoService,
        fuseConfirmationService: FuseConfirmationService, route: ActivatedRoute,
        @Inject('BASE_URL') baseUrl: string,
        @Inject(MAT_DIALOG_DATA) data: any) {
        super(_translocoService, fuseConfirmationService, route, baseUrl, http, 'sys_approval_config', dialogRef);
        this.record = data;
        this.Oldrecord = JSON.parse(JSON.stringify(data));
        this.resetAddItem();
        this.actionEnum = data.actionEnum;
        this.listMenu = [{
            id: 'inventory_delivery',
            name: this._translocoService.translate("NAV.inventory_delivery")
        },
        {
            id: 'inventory_receiving',
            name: this._translocoService.translate("NAV.inventory_receiving")
        },
        {
            id: 'buyorder_purchase_order',
            name: this._translocoService.translate("NAV.buyorder_purchase_order")
        },
        {
            id: 'business_sale_order',
            name: this._translocoService.translate("NAV.business_sale_order")
        }
            ,
        {
            id: 'production_order',
            name: this._translocoService.translate("NAV.production_order")
        }
            ,
        {
            id: 'maintenance_planning',
            name: this._translocoService.translate("NAV.maintenance_planning")
        }
            ,
        {
            id: 'maintenance_process',
            name: this._translocoService.translate("NAV.maintenance_process")
        }
            ,
        {
            id: 'maintenance_repair_process',
            name: this._translocoService.translate("NAV.maintenance_repair_process")
        },
        {
            id: 'inventory_request_transfer',
            name: this._translocoService.translate("NAV.inventory_request_transfer")
        }, {
            id: 'inventory_process_transfer',
            name: this._translocoService.translate("NAV.inventory_process_transfer")

        }, {
            id: 'inventory_request_export',
            name: this._translocoService.translate("NAV.inventory_request_export")

        }, {
            id: 'inventory_check_planning',
            name: this._translocoService.translate("NAV.inventory_check_planning")
        },
        {
            id: 'inventory_check_process',
            name: this._translocoService.translate("NAV.inventory_check_process")
        },
        {
            id: 'tool_write_increase',
            name: this._translocoService.translate("NAV.tool_write_increase")
        },
        {
            id: 'tool_broken_voucher',
            name: this._translocoService.translate("NAV.tool_broken_voucher")
        },
        {
            id: 'tool_write_reduced',
            name: this._translocoService.translate("NAV.tool_write_reduced")
        },
        {
            id: 'tool_request_transfer',
            name: this._translocoService.translate("NAV.tool_request_transfer")
        },
        {
            id: 'tool_transfer_process',
            name: this._translocoService.translate("NAV.tool_transfer_process")
        },
        {
            id: 'tool_write_borrow',
            name: this._translocoService.translate("NAV.tool_write_borrow")
        },
        {
            id: 'tool_write_return',
            name: this._translocoService.translate("NAV.tool_write_return")
        },
        {
            id: 'buyorder_request_order',
            name: this._translocoService.translate("NAV.buyorder_request_order")
        }]
        if (this.actionEnum == 1) {
            this.baseInitData();
        }

        if (this.actionEnum != 1) {
            this.record.form_name = this._translocoService.translate("NAV." + this.record.db.menu);
            this.http
                .post('/sys_approval_config.ctr/getListItem/', {
                    id: this.record.db.id
                }
                ).subscribe(resp => {
                    this.record.list_item = resp;
                });


        }

    }

    resetAddItem(): void {
        this.additem = {
            db: {
                user_id: null,
                step_num: 0,
                name: "",
                note: "",

            },
            user_name: "",
        };

    }
    bind_data_item_chose(): void {
        this.additem.db.user_id = this.item_chose.id;
        this.additem.user_name = this.item_chose.name;
    }
    addDetail(): void {

        var error = '';

        if (this.record.list_item.filter(d => d.db.user_id == this.additem.db.user_id && d.db.step_num == this.additem.db.step_num).length > 0) {
            error += this._translocoService.translate('existed');

            this.showMessageWarningNoConfirm(error);
            return;
        }

        if (this.additem.db.user_id == null || this.additem.db.user_id == undefined) {
            error += this._translocoService.translate('must_chose_item');

            this.showMessageWarningNoConfirm(error);
            return;
        }

        if (this.additem.db.step_num == 0 || this.additem.db.step_num == null || this.additem.db.step_num == '') {
            error += this._translocoService.translate('system.step_num') + " " + this._translocoService.translate('must_chose_item');

            this.showMessageWarningNoConfirm(error);
            return;
        }


        this.record.list_item = [... this.record.list_item, this.additem]
        this.record.list_item.sort(function (a, b) {
            return a.db.step_num - b.db.step_num;
        });

        this.resetAddItem();
    }
    deleteDetail(pos): void {
        this.record.list_item.splice(pos, 1);
        this.record.list_item = [...this.record.list_item]
    }
}
