import { Component, Inject, ViewChild, OnInit, OnDestroy  } from '@angular/core';


import { HttpClient, HttpResponse } from '@angular/common/http';

import { DataTablesResponse } from 'app/Basecomponent/datatable';

import { TranslocoService } from '@ngneat/transloco';
import { MatDialog } from '@angular/material/dialog';

import { Subject, timer } from 'rxjs';
import { BaseIndexDatatableComponent } from 'app/Basecomponent/BaseIndexDatatable.component';

import { FuseConfirmationService } from '@fuse/services/confirmation';
import { ActivatedRoute } from '@angular/router';


@Component({
    selector: 'sys_opc_view_index',
    templateUrl: './index.component.html',
    styleUrls: ['./index.component.scss']
})

export class sys_opc_viewComponent implements OnInit, OnDestroy
{



    private timer;
    public workstation_chose: any;
    public list_workstation: any
    public production_order_item: any;
    public workstation_template_chose: any;
    public list_workstation_template_opc: any
    public workstation_config: any = {
        list_param:[],
    };
    public list_production_order: any = [];
    public list_production_order_item: any = [];

    public filter={
        id_workstation: "",
        id_template_opc: "",

    };


    constructor(private http: HttpClient, dialog: MatDialog
        , private _translocoService: TranslocoService
        , fuseConfirmationService: FuseConfirmationService, route: ActivatedRoute
        ) {



        this.http
            .post('/sys_workstation.ctr/getListUse/', {
            }
            ).subscribe(resp => {
                this.list_workstation = resp;
            });
        this.http
            .post('/sys_opc_view.ctr/getProductionOrder/', {
            }
        ).subscribe(resp => {
            this.list_production_order = resp;
            });

    }
    public showMessagewarning(msg): void {
        // Swal.fire({
        //     title: msg,
        //     text: "",
        //     icon: 'warning',
        //     confirmButtonColor: '#3085d6',
        //     confirmButtonText: this._translocoService.translate('close'),
        // }).then((result) => {

        // })

    }
   workstation_change(): void {

       this.http.post('/sys_workstation_template_opc.ctr/getListUse', {
           id_workstation:this.workstation_chose.id
       }
        ).subscribe(resp => {
            this.list_workstation_template_opc = resp;
        });
    }
    workstation_template_change(): void {
        this.http.post('/sys_template_opc.ctr/getopcconfig', {
            id_template_opc: this.workstation_template_chose.id
        }
        ).subscribe(resp => {
            this.workstation_config = resp;
        });
    }

    ngOnInit() {

    }
    ngOnDestroy() {
        clearInterval(this.timer);
    }
    routerOnDeactivate() {

    }

    perform(item, pos) {
        if (this.workstation_template_chose == undefined) {

            //this.showMessageWarningNoConfirm("system.must_chose_template_opc"))
            return;
        }
        for (var i = 0; i < this.list_production_order.length; i++) {
            if (this.list_production_order[i].status == 2) {
                this.list_production_order[i].status == 3
                this.list_production_order[i].value_output =this.workstation_config.value_output;
                this.list_production_order[i].value_input =this.workstation_config.value_input;
                this.list_production_order[i].value_error = this.workstation_config.value_error;
            }
        }
        debugger;
        item.run_status = 2;
    }

    stop(item, pos) {
        item.value_output = this.workstation_config.value_output;
        item.value_input =  this.workstation_config.value_input;
        item.value_error =  this.workstation_config.value_error;
        item.run_status = 3;
    }




}


