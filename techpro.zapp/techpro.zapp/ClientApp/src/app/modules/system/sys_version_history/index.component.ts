import { Component, Inject, ViewChild } from '@angular/core';


import { HttpClient, HttpResponse } from '@angular/common/http';

import { DataTablesResponse } from 'app/Basecomponent/datatable';
import { TranslocoService } from '@ngneat/transloco';
import { MatDialog } from '@angular/material/dialog';

import { Subject } from 'rxjs';
import { BaseIndexDatatableComponent } from 'app/Basecomponent/BaseIndexDatatable.component';

import { FuseConfirmationService } from '@fuse/services/confirmation';
import { ActivatedRoute } from '@angular/router';


@Component({
    selector: 'sys_version_history_index',
    templateUrl: './index.component.html',
})

export class sys_version_history_indexComponent extends BaseIndexDatatableComponent
{
    // public listStatusDel: CmSelectType[];
    public list_help: any = [];

    constructor(http: HttpClient, dialog: MatDialog
        , _translocoService: TranslocoService
        , fuseConfirmationService: FuseConfirmationService,route: ActivatedRoute
        , @Inject('BASE_URL') baseUrl: string
        ) {
        super(http, baseUrl, _translocoService, fuseConfirmationService, route, dialog,'sys_version_history',
            {  }
        )


    }


    ngOnInit(): void {
        // this.baseInitData();
        this.http.get('sys_version.ctr/getAll').subscribe((resp: any[]) => {
            this.listData = resp;
        });
    }


}


