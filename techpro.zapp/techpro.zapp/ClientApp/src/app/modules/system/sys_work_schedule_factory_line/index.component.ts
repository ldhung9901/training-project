import { Component, Inject, ViewChild } from '@angular/core';


import { HttpClient, HttpResponse } from '@angular/common/http';

import { DataTablesResponse } from 'app/Basecomponent/datatable';

import { TranslocoService } from '@ngneat/transloco';
//import { sys_work_schedule_factory_linepopUpAddComponent } from './popupAdd.component';
import { MatDialog } from '@angular/material/dialog';

import { Subject } from 'rxjs';
import { BaseIndexDatatableComponent } from 'app/Basecomponent/BaseIndexDatatable.component';

import { FuseConfirmationService } from '@fuse/services/confirmation';
import { ActivatedRoute } from '@angular/router';


@Component({
    selector: 'sys_work_schedule_factory_lineindex',
    templateUrl: './index.component.html',
    styleUrls: ['./index.component.scss']
})

export class sys_work_schedule_factory_lineindexComponent extends BaseIndexDatatableComponent
{
    listfactory: any;
    item_chose_factory: any;
    public listfactoryline: any;
    public errorModel: any;
    public list_trang_thai: any;
    public trang_thais: any;
    public list_du_an: any;
    public list_cong_ty: any;
    public attributes: any;
    public list_attributes: any;
    public loaidulieus: any;
    public list_loaidulieus: any;
    public list_cauhinh: any;
    public loading: boolean = false;
    constructor(http: HttpClient, dialog: MatDialog
        , _translocoService: TranslocoService
        , fuseConfirmationService: FuseConfirmationService,route: ActivatedRoute
        , @Inject('BASE_URL') baseUrl: string
        ) {
        super(http, baseUrl, _translocoService, fuseConfirmationService, route, dialog,'sys_work_schedule_factory_line',
            { search: "", id_sys_factory_line: "", id_sys_factory: ""}
        )

        this.http
            .post('/sys_factory.ctr/getListUse/', {}
            ).subscribe(resp => {
                this.listfactory = resp;
            });





    }
    bind_data_factory(): void {
        this.filter.id_sys_factory_line = "";
        this.rerender();
        this.http
            .post('/sys_factory_line.ctr/getListUse/', {
                id_factory: this.filter.id_sys_factory
            }
            ).subscribe(resp => {
                this.listfactoryline = resp;
            });
    }
    deleteDetail(pos): void {
        this.listData.splice(pos, 1);
    }

    handleDataBefor(): void {

        for (var i = 0; i < this.listData.length; i++) {

            if (this.listData[i].db.timeStart_1 != null) {
                this.listData[i].db.timeStart_1 = this._formatDatetime(new Date(this.listData[i].db.timeStart_1), "hh:ii");
            }
            if (this.listData[i].db.timeStart_2 != null) {
                this.listData[i].db.timeStart_2 = this._formatDatetime(new Date(this.listData[i].db.timeStart_2), "hh:ii");
            }
            if (this.listData[i].db.timeStart_3 != null) {
                this.listData[i].db.timeStart_3 = this._formatDatetime(new Date(this.listData[i].db.timeStart_3), "hh:ii");
            }
            if (this.listData[i].db.timeEnd_1 != null) {
                this.listData[i].db.timeEnd_1 = this._formatDatetime(new Date(this.listData[i].db.timeEnd_1), "hh:ii");
            }
            if (this.listData[i].db.timeEnd_2 != null) {
                this.listData[i].db.timeEnd_2 = this._formatDatetime(new Date(this.listData[i].db.timeEnd_2), "hh:ii");
            }
            if (this.listData[i].db.timeEnd_3 != null) {
                this.listData[i].db.timeEnd_3 = this._formatDatetime(new Date(this.listData[i].db.timeEnd_3), "hh:ii");
            }
        }

    }
    reset(item): void {
        this.showConfirmDialog().afterClosed().subscribe((result) => {
            if (result === 'confirmed') {
                this.errorModel = [];
                for (var i = 0; i < this.listData.length; i++) {

                    var dataCopy = this.listData[i].db;
                    dataCopy.timeStart_1 = null;
                    dataCopy.timeStart_2 = null;
                    dataCopy.timeStart_3 = null;
                    dataCopy.timeEnd_1 = null;
                    dataCopy.timeEnd_2 = null;
                    dataCopy.timeEnd_3 = null;
                    dataCopy.note = null;

                }
                this.showMessageSuccessNoConfirm('Reset thành công');

            }
        })

    };
    saoChep(item): void {
        this.showConfirmDialog().afterClosed().subscribe((result) => {
            if (result === 'confirmed') {

                for (var i = 0; i < this.listData.length; i++) {
                    if (i != 0) {
                        var dataCopy = this.listData[i].db;
                        dataCopy.timeStart_1 = item.db.timeStart_1;
                        dataCopy.timeStart_2 = item.db.timeStart_2;
                        dataCopy.timeStart_3 = item.db.timeStart_3;
                        dataCopy.timeEnd_1 = item.db.timeEnd_1;
                        dataCopy.timeEnd_2 = item.db.timeEnd_2;
                        dataCopy.timeEnd_3 = item.db.timeEnd_3;
                    }


                }
                this.showMessageSuccessNoConfirm('Sao chép thành công');

            }
        })


    }
    saveConfig() {
        if (this.filter.id_sys_factory == "") {
            this.showMessageWarningNoConfirm("system.must_chose_factory")
            return
        }
        if (this.filter.id_sys_factory_line == "") {
            this.showMessageWarningNoConfirm("system.must_chose_factory_line")
            return
        }
        this.showConfirmDialog().afterClosed().subscribe((result) => {
            if (result === 'confirmed') {
                this.http
                    .post('sys_work_schedule_factory_line.ctr/create/',
                        {
                            data: this.listData,
                            id_sys: {
                                id_sys_factory: this.filter.id_sys_factory,
                                id_sys_factory_line: this.filter.id_sys_factory_line
                            }


                        }
                    ).subscribe(resp => {
                        this.errorModel = [];
                        this.rerender();
                        this.showMessageSuccessNoConfirm('Cấu hình thành công');
                    },
                        error => {
                            if (error.status == 400) {
                                this.errorModel = error.error;

                            }
                            //this.loading = false;
                        }
                    );
            }
        })

    }
    ngOnInit(): void {
        //this.baseInitData(this.pageIndex,this.pageSize,null,null,[]);



    }


}


