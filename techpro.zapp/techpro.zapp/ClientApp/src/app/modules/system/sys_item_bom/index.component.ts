import { Component, EventEmitter, HostListener, Inject, Output, ViewChild } from '@angular/core';


import { HttpClient, HttpResponse } from '@angular/common/http';

import { DataTablesResponse } from 'app/Basecomponent/datatable';

import { TranslocoService } from '@ngneat/transloco';
import { sys_item_bom_popUpAddComponent } from './popupAdd.component';
import { sys_item_bom_popupAdd_configComponent } from './popupAddConfig.component';
import { MatDialog } from '@angular/material/dialog';

import { Subject } from 'rxjs';
import { BaseIndexDatatableComponent } from 'app/Basecomponent/BaseIndexDatatable.component';

import { FuseConfirmationService } from '@fuse/services/confirmation';
import { ActivatedRoute } from '@angular/router';
import { sys_item_bom_treeviewComponent } from './treeview.component';
import { actionEnums } from 'app/Basecomponent/actionEnums';
import { sys_item_bom_config_model, sys_item_bom_model } from './type';
import { sys_item_bom_detail_popUpAddComponent } from './sys_item_bom_detail/popupAdd.component';
import { isThisSecond } from 'date-fns';
import { MatDrawer } from '@angular/material/sidenav';
import { takeUntil } from 'rxjs/operators';
import { FuseMediaWatcherService } from '@fuse/services/media-watcher';

@Component({
    selector: 'sys_item_bom_index',
    templateUrl: './index.component.html',
    styleUrls: ['./index.component.scss']
})

export class sys_item_bom_indexComponent extends BaseIndexDatatableComponent {
    item_chose: any;
    idConfig: number;
    list_bom_config: sys_item_bom_model;
    public item_chose_config: any;
    idItem: any;
    // drawer attribute shrink.
    drawerMode: 'over' | 'side' = 'side';
    drawerOpened: boolean = true;
    @ViewChild('drawer') drawer: MatDrawer;
    // Close drawer when resize screeen
    @HostListener('window:resize', ['$event.target.innerWidth'])
    onResize(width : number) {
        this.drawerOpened = width >= 960
    }
    // import for Responsice
    private _unsubscribeAll: Subject<any> = new Subject<any>();
    private _fuseMediaWatcherService: FuseMediaWatcherService

    @Output() modelChange: EventEmitter<any> = new EventEmitter<any>();
    data: sys_item_bom_config_model;
    constructor(http: HttpClient, dialog: MatDialog
        , _translocoService: TranslocoService
        , fuseConfirmationService: FuseConfirmationService, route: ActivatedRoute
        , @Inject('BASE_URL') baseUrl: string
    ) {
        super(http, baseUrl, _translocoService, fuseConfirmationService, route, dialog, 'sys_item_bom',
            { search: "", id_item: "", id_item_bom_config: 0 }
        )
        this.getListConfig()


    }

    getListConfig() {
        this.http.get<sys_item_bom_model>('sys_item_bom.ctr/getAll/', {}).subscribe(resp => {

            this.list_bom_config = resp

        })
    }
    // bind_data_item(): void {
    //     this.filter.id_item_bom_config = 0;
    //     this.http
    //         .post('/sys_item_bom.ctr/get_bom_config/', {
    //             id_item: this.filter.id_item
    //         }
    //         ).subscribe(resp => {
    //             this.list_bom_config = resp;
    //         });
    //         this.getListConfig();
    // }

    openDialogAdd(): void {
        if (this.filter.id_item == "" || this.filter.id_item == undefined) {
            this.showMessageWarningNoConfirm('system.must_chose_item')

            return;
        }
        if (this.filter.id_item_bom_config == "" || this.filter.id_item_bom_config == undefined) {
            this.showMessageWarningNoConfirm('system.must_chose_bom_config')

            return;
        }

        const dialogRef = this.dialog.open(sys_item_bom_popUpAddComponent, {
            disableClose: true,
            data: {
                actionEnum: 1,
                db: {
                    id: 0,
                    id_item: this.item_chose.id,
                    id_unit: this.item_chose.id_unit,
                    id_item_bom_config: this.item_chose_config.id
                },
                sys_item_name: this.item_chose.name,
                sys_item_unit_name: this.item_chose.unit_name,
                item_specification_name: this.item_chose_config.specification_name,
                config_name: this.item_chose_config.name
            },
        });
        dialogRef.afterClosed().subscribe(result => {
            if (result.db.id == 0) return;
            this.rerender();
        });
    }

    openDialogAddConfig(): void {
        const dialogRef = this.dialog.open(sys_item_bom_detail_popUpAddComponent, {
            disableClose: true,
            data: {
                actionEnum: 1,
                db: {
                    id: 0,
                    status_use: true,


                },

            },
        });
        dialogRef.afterClosed().subscribe(result => {
            if (result.db.id == 0) return;
            this.getListConfig();
        });
    }

    openDialogEdit(model: sys_item_bom_config_model): void {

        model.actionEnum = actionEnums.Edit;
        const dialogRef = this.dialog.open(sys_item_bom_detail_popUpAddComponent, {
            disableClose: true,
            data: model
        });
        dialogRef.afterClosed().subscribe(result => {
            if (result != undefined && result != null)
                model.actionEnum = actionEnums.Detail
        });

    }
    openDialogDetail(model, pos): void {
        model.actionEnum = actionEnums.Detail;
        const dialogRef = this.dialog.open(sys_item_bom_popUpAddComponent, {
            disableClose: true,
            data: model
        });
        dialogRef.afterClosed().subscribe(result => {
            if (result != undefined && result != null) this.listData[pos] = result;
        });
    }

    selectBomConfig(model: sys_item_bom_config_model): void {
        // idSelect = model.
        this.idConfig = model.db.id;
        this.idItem = model.db.id_item;
        model.actionEnum = 3;
        this.data = model;


    }

    ngOnInit(): void {
        //this.baseInitData(this.pageIndex,this.pageSize,null,null,[]);
        this._fuseMediaWatcherService.onMediaChange$
            .pipe(takeUntil(this._unsubscribeAll))
            .subscribe(({ matchingAliases }) => {

                // Set the drawerMode and drawerOpened if the given breakpoint is active
                if (matchingAliases.includes('md')) {
                    this.drawerMode = 'side';
                    this.drawerOpened = true;
                }
                else {
                    this.drawerMode = 'over';
                    this.drawerOpened = false;
                }
            });
    }
    ngOnDestroy(): void {
        // Unsubscribe from all subscriptions
        this._unsubscribeAll.next();
        this._unsubscribeAll.complete();
    }

    public deleteBomConfig(id): void {
        this.showConfirmDialog().afterClosed().subscribe((result) => {
            if (result === 'confirmed') {
                this.http
                    .post('/sys_item_bom.ctr/delete_bom_config/', {
                        id: id,
                    }
                    ).subscribe(resp => {
                        this.getListConfig();
                    });
            }
        })

    }
    enableEdit(): void {
        this.data.actionEnum = actionEnums.Edit;
    }
    trackByFn(index, item): any {


        return item.id || index;
    }
    receive(id): void {
        console.log(id);

    }
    saveConfig(): void {

        this.data.actionEnum = 3;
    }

}


