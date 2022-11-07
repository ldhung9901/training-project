import { Component, Inject, ViewChild } from '@angular/core';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { TranslocoService } from '@ngneat/transloco';
import { MatDialog } from '@angular/material/dialog';
import { BaseIndexDatatableComponent } from 'app/Basecomponent/BaseIndexDatatable.component';
import { FuseConfirmationService } from '@fuse/services/confirmation';
import { ActivatedRoute } from '@angular/router';
import { sys_help_page_detailComponent } from './detail.component';
import { CmSelectType } from '@fuse/components/commonComponent/cm_select_component/types';


// import { sys_help_page_popUpAddComponent } from './popupAdd.component';

@Component({
    selector: 'sys_help_page_index',
    templateUrl: './index.component.html',
    styleUrls: ['./index.component.scss']
})

export class sys_help_page_indexComponent extends BaseIndexDatatableComponent {

    public list_customer_type: any;
    public listStatusDel: CmSelectType[];
    list_course: any;
    list_help: any;
    // filter: {
    //     // Base filter
    //     search: '',
    //     id_help: ''
    // }
    constructor(http: HttpClient, dialog: MatDialog
        , _translocoService: TranslocoService
        , fuseConfirmationService: FuseConfirmationService, route: ActivatedRoute
        , @Inject('BASE_URL') baseUrl: string
    ) {
        super(http, baseUrl, _translocoService, fuseConfirmationService, route, dialog, 'sys_help_detail',
            { search: "", status_del: "1", id_help: "" }
        )

        this.http.post("/sys_help.ctr/getListUse/", {}).subscribe((resp) => {
            this.list_help = resp;
            this.list_help.splice(0, 0, { id: '-1', name: this._translocoService.translate('all') });

        });
        this.filter = {
            search: '',
            id_help: '-1',
            status_del: 1,
        };
    }

    openDialogAdd(i): void {
        const dialogRef = this.dialog.open(sys_help_page_detailComponent, {
            disableClose: true,
            panelClass: 'my-full-screen-dialog',
            data: {
                actionEnum: 3,
                db: {
                    id: i,
                }
            },
        });
        dialogRef.afterClosed().subscribe(result => {
            if (result.db.id == 0) return;
            this.rerender();
        });
    }

    ngOnInit(): void {
        this.http.get('sys_help_detail.ctr/getAll').subscribe((resp: any[]) => {
            this.listData = resp;

        });

    }

}


