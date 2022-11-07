import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef, MatDialog } from '@angular/material/dialog';

import { HttpClient } from '@angular/common/http';
import { BasePopUpAddComponent } from 'app/Basecomponent/BasePopupAdd.component';
import { TranslocoService } from '@ngneat/transloco';
import { FuseConfirmationService } from '@fuse/services/confirmation';
import { ActivatedRoute } from '@angular/router';

import { sys_item_min_max_stock_model, sys_item_model, sys_item_quality, sys_item_quality_detail_db, sys_item_unit_other_model } from './types';
import { sys_item_popUpAddDetailComponent } from './popupAddDetail.component';
import moment from 'moment';


@Component({
    selector: 'sys_item_popupAdd',
    templateUrl: 'popupAdd.html',
})
export class sys_item_popUpAddComponent extends BasePopUpAddComponent {

    listSysItemType: { id: string; name: string }[] = [];
    listSysUnit: { id: string; name: string }[] = [];
    listSysWarehouse: { id: string; name: string }[] = [];
    listCostType: { id: string, name: string, cost_type_code: string }[] = [];
    search = "";
    searchItemUnitOther: "";
    searchWarehouse: "";
    listType: { id: number, name: string }[] = [];
    additem: any = {
        id_unit: '',
        conversion_factor: '',
    };

    additem_min_max: any = {
        id_warehouse: '',
        min_stock: '',
        max_stock: ''
    };
    record: sys_item_model;
    listQuanlity: sys_item_quality;
    constructor(public dialogRef: MatDialogRef<sys_item_popUpAddComponent>,
        public dialog: MatDialog,
        http: HttpClient, _translocoService: TranslocoService,
        fuseConfirmationService: FuseConfirmationService, route: ActivatedRoute,
        @Inject('BASE_URL') baseUrl: string,
        @Inject(MAT_DIALOG_DATA) data: any) {
        super(_translocoService, fuseConfirmationService, route, baseUrl, http, 'sys_item', dialogRef);

        this.record = data;


        this.Oldrecord = JSON.parse(JSON.stringify(data));
        this.actionEnum = data.actionEnum;
        this.listType = [
            {
                'id': 1,
                'name': this._translocoService.translate('system.product'),
            },
            {
                'id': 2,
                'name': this._translocoService.translate('system.semi_product'),
            },
            {
                'id': 3,
                'name': this._translocoService.translate('system.supplies'),
            },
            {
                'id': 4,
                'name': this._translocoService.translate('system.tool'),
            }
        ]
        if (this.record.db.type == 1) this.record.type_name = this._translocoService.translate('system.product');
        else if (this.record.db.type == 2) this.record.type_name = this._translocoService.translate('system.semi_product');
        else if (this.record.db.type == 3) this.record.type_name = this._translocoService.translate('system.supplies');


        if (this.actionEnum == 1) {
            this.baseInitData();
            this.addMinMaxStock();
            this.addUnit();
        }
        else {
            //Data item
            this.getListQuanlity();
            this.getListMinMaxStock();
            this.getListUnitOther();

        }
        this.getListItemType();//Loại
        this.getListWarehouse();//Kho
        this.getListCostType(); //Nhóm chi phí
        this.getListUnit();//Đơn vị tính
    }
    // Quan li chat luong
    getListQuanlity() {
        this.http
            .post('/sys_item.ctr/getListQuanlity/', {
                id: this.record.db.id
            }
            ).subscribe((resp: sys_item_quality[]) => {
                if (resp) {
                    this.record.list_item_quality = resp;
                } else {
                    this.record.list_item_quality = [];

                }


            });
    }
    //Nhóm chi phí
    getListCostType() {
        this.http
            .post('/sys_cost_type.ctr/getListUse/', {}
            ).subscribe((resp: { id: string, name: string, cost_type_code: string }[]) => {
                this.listCostType = resp;
            });
    }
    //Đơn vị tính
    getListUnit() {
        this.http
            .post('/sys_unit.ctr/getListUse/', {}
            ).subscribe((resp: { id: string; name: string }[]) => {
                this.listSysUnit = resp;
            });
    }

    //Đơn vị tính khác
    getListUnitOther() {
        this.http
            .post('/sys_item.ctr/getListUnitOther/', {
                id: this.record.db.id
            }
            ).subscribe((resp: sys_item_unit_other_model[]) => {
                this.record.list_sys_item_unit_other = resp;
                if (resp.length == 0) {
                    this.addUnit();
                }
            });
    }
    //Max xin tồn kho
    getListMinMaxStock() {
        this.http
            .post('/sys_item.ctr/getListMinMaxStock/', {
                id: this.record.db.id
            }
            ).subscribe((resp: sys_item_min_max_stock_model[]) => {
                this.record.list_item_min_max_stock = resp;
                if (resp.length == 0) {
                    this.addMinMaxStock();
                }
            });
    }
    //Nhóm vật tư
    getListItemType() {
        this.http
            .post('/sys_item_type.ctr/getListUse/', {}
            ).subscribe((resp: { id: string; name: string }[]) => {
                this.listSysItemType = resp;
            });
    }
    getListWarehouse() {
        this.http
            .post('/sys_warehouse.ctr/getListUse/', {}
            ).subscribe((resp: { id: string; name: string }[]) => {
                this.listSysWarehouse = resp;
            });
    }

    openDialogAdd(): void {

        const dialogRef = this.dialog.open(sys_item_popUpAddDetailComponent, {
            disableClose: true,
            width: '90%',
            data: <sys_item_quality>{
                actionEnum: 1,
                db: {
                    id_item: this.record.db.id,
                    status_del : 1,
                    create_date: moment().toISOString(),
                    status_use: false
                },
                list_item_quality: [],
            },
        });
        dialogRef.afterClosed().subscribe(result => {
            if (result != undefined && result != null) {

                // this.record.list_item_quality = result;
                this.record.list_item_quality = [...this.record.list_item_quality, result]
            }

        });
    }
    openDialogDetail(model: sys_item_quality, pos: number) {
        model.actionEnum = 3;
        const dialogRef = this.dialog.open(sys_item_popUpAddDetailComponent, {
            disableClose: true,
            data: <sys_item_quality>model
        });
        dialogRef.afterClosed().subscribe(result => {
            if (result != undefined && result != null) {
                this.record.list_item_quality[pos] = result;
            }

        });
    }
    openDialogEdit(model: sys_item_quality, pos: number) {
        // console.log(model);

        model.actionEnum = 2;
        const dialogRef = this.dialog.open(sys_item_popUpAddDetailComponent, {
            disableClose: true,
            data: <sys_item_quality>model
        });
        dialogRef.afterClosed().subscribe(result => {
            if (result != undefined && result != null) {
                console.log(result);
                this.record.list_item_quality[pos] = result;

            }

        });
    }
    selectFile(event): void {

        if (event.target.files.length > 0) {
            if (event.target.files[0].type.split("/")[0] == "image") {
                var formData = new FormData();
                formData.append('folder', this.controller);
                formData.append('file', event.target.files[0]);

                this.http
                    .post("fileManager.ctr/upload_file",
                        formData
                    ).subscribe((resp) => {
                        this.record.db.pathImg = resp.toString();
                    },
                        error => {
                            if (error.status == 400) {
                                this.errorModel = error.error;

                            }
                        }
                    );
            }
            else {
                this.showMessageWarningNoConfirm('system.please_choose_picture')
            }
        }

    }
    removeItemImg() {
        this.record.db.pathImg = null;
    }

    delete(id: sys_item_quality[], pos: number): void {

        this.fuseConfirmationService.open({
            title: 'areYouSure',
            message: '',
            icon: {
                name: 'heroicons_outline:trash',
                color: 'error'
            }
        }).afterClosed().subscribe((result) => {

            // If the confirm button pressed...
            if (result === 'confirmed') {
                this.http
                    .post(this.controller + '.ctr/delete_item/',
                        {
                            id: id,
                        }
                    ).subscribe(resp => {
                        this.record.list_item_quality.splice(pos, 1)
                        this.record.list_item_quality = [...this.record.list_item_quality]
                    });
            }
        });
        this.getListQuanlity()


    }
    public beforesave(): boolean {
        //console.log(this.record)
        let checkExistedMinMaxStock = false;
        let checkExistedUnitOther = false;
        let checkdCompareMinMaxStock = false;
        this.record.list_item_min_max_stock.forEach(item => {
            if (this.record.list_item_min_max_stock.filter(t => t.db.id_warehouse == item.db.id_warehouse).length >= 2) {
                checkExistedMinMaxStock = true;
                this.showMessageWarningNoConfirm('system.erorr_min_max_stock_duplicate');
                return;
            }
            if (this.record.list_item_min_max_stock.filter(t => t.db.max_stock < item.db.min_stock).length > 0) {
                checkdCompareMinMaxStock = true;
                this.showMessageWarningNoConfirm('system.error_Compare__min_max');
                return;
            }
        });
        if (checkExistedMinMaxStock || checkdCompareMinMaxStock) {
            return false;
        }
        this.record.list_sys_item_unit_other.forEach(item => {
            if (this.record.list_sys_item_unit_other.filter(t => t.db.id_unit == item.db.id_unit).length >= 2) {
                checkExistedUnitOther = true;
                this.showMessageWarningNoConfirm('system.erorr_unit_other_duplicate');
                return;
            }
        });
        if (checkExistedUnitOther) {
            return false;
        }


        //return false;
        // this.record.list_item_min_max_stock= this.record.list_item_min_max_stock.filter(t=>t.db.id_warehouse);
        //this.record.list_sys_item_unit_other= this.record.list_sys_item_unit_other.filter(t=>t.db.id_unit);

        // if ((this.record.db.price ?? 0) < 0) {
        //     Swal.fire({
        //         title: this._translocoService.translate('not_below_than_zero'),
        //         text: '',
        //         icon: 'warning',
        //         confirmButtonColor: '#3085d6',
        //         confirmButtonText: this._translocoService.translate('close')
        //     }).then((result) => { });
        //     return false;
        // }
        return true;

    }

    addUnit(): void {
        this.record.list_sys_item_unit_other.push({
            sys_unit_name: "",
            db: {
                id: 0,
                conversion_factor: null,
                id_item: null,
                id_unit: null,
                note: null
            }
        });
    }

    removeUnit(index: number): void {
        this.record.list_sys_item_unit_other.splice(index, 1);
    }

    addMinMaxStock(): void {
        this.record.list_item_min_max_stock.push({
            sys_warehouse_name: "",
            db: {
                id: 0,
                id_warehouse: null,
                max_stock: 0,
                min_stock: 0,
                id_item: null,

            }
        });
    }
    changeMinMaxStock(item) {
        if (item.db.min_stock == null) {
            item.db.min_stock = 0;
        }
        if (item.db.max_stock == null) {
            item.db.max_stock = 0;
        }
    }
    changeUnitOther(item) {
        if (item.db.conversion_factor == null) {
            item.db.conversion_factor = 0;
        }
    }

    removeMinMaxStock(index: number): void {
        this.record.list_item_min_max_stock.splice(index, 1);
    }

    changeStatus(event) {
        if (event.checked) {
            this.record.db.status_del = 1;
        }
        if (!event.checked) {
            this.record.db.status_del = 2;
        }
    }
    trackByFn(index: number, item: any): any {
        return item.id || index;
    }
}
