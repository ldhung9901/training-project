import { ChangeDetectorRef, Component, Inject, ViewChild } from '@angular/core';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { DataTablesResponse } from 'app/Basecomponent/datatable';
import { TranslocoService } from '@ngneat/transloco';
import { MatDialog } from '@angular/material/dialog';
import { Subject } from 'rxjs';
import { BaseIndexDatatableComponent } from 'app/Basecomponent/BaseIndexDatatable.component';
import { FuseConfirmationService } from '@fuse/services/confirmation';
import { ActivatedRoute } from '@angular/router';
import { actionEnums } from 'app/Basecomponent/actionEnums';
import { CmSelectType } from '@fuse/components/commonComponent/cm_select_component/types';
import writeXlsxFile from 'write-excel-file';
import moment from 'moment';
import { MatDrawer } from '@angular/material/sidenav';
import { takeUntil } from 'rxjs/operators';
import { FuseMediaWatcherService } from '@fuse/services/media-watcher';

@Component({
    selector: 'sys_configuration_index',
    templateUrl: './index.component.html',
})

export class sys_configuration_indexComponent extends BaseIndexDatatableComponent
{
    @ViewChild('drawer') drawer: MatDrawer;
    drawerMode: 'over' | 'side' = 'side';
    drawerOpened: boolean = true;
    panels: any[] = [];
    selectedPanel: string = 'account';
    public currentLevel = 0;

    private _unsubscribeAll: Subject<any> = new Subject<any>();

    constructor(http: HttpClient, dialog: MatDialog,
        private _changeDetectorRef: ChangeDetectorRef,
        private _fuseMediaWatcherService: FuseMediaWatcherService
        , _translocoService: TranslocoService
        , fuseConfirmationService: FuseConfirmationService, route: ActivatedRoute
        , @Inject('BASE_URL') baseUrl: string
        ) {
        super(http, baseUrl, _translocoService, fuseConfirmationService, route, dialog,'sys_configuration',
            { }
        )

    }
    ngOnInit(): void
    {
        // Setup available panels
        this.panels = [
            {
                id         : 'account',
                icon       : 'heroicons_outline:office-building',
                title      : 'Thông tin công ty',
                description: 'Quản lý và cấu hình thông tin công ty'
            },
            {
                id         : 'security',
                icon       : 'heroicons_outline:document-text',
                title      : 'Định dạng chứng từ',
                description: 'Định dạng số chứng từ tự sinh khi lập phiếu'
            },
            {
                id         : 'plan-billing',
                icon       : 'heroicons_outline:calendar',
                title      : 'Thời gian làm việc',
                description: 'Quản lý và cấu hình thời gian làm việc'
            }
        ];

    }
    /**
     * On destroy
     */
     ngOnDestroy(): void
     {
         // Unsubscribe from all subscriptions
         this._unsubscribeAll.next();
         this._unsubscribeAll.complete();
     }

     // -----------------------------------------------------------------------------------------------------
     // @ Public methods
     // -----------------------------------------------------------------------------------------------------

     /**
      * Navigate to the panel
      *
      * @param panel
      */
     goToPanel(panel: string): void
     {
         this.selectedPanel = panel;

         // Close the drawer on 'over' mode
         if ( this.drawerMode === 'over' )
         {
             this.drawer.close();
         }
     }

     /**
      * Get the details of the panel
      *
      * @param id
      */
     getPanelInfo(id: string): any
     {
         return this.panels.find(panel => panel.id === id);
     }

     /**
      * Track by function for ngFor loops
      *
      * @param index
      * @param item
      */
     trackByFn(index: number, item: any): any
     {
         return item.id || index;
     }

}


