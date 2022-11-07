import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';

import { HttpClient } from '@angular/common/http';
import { BasePopUpAddComponent } from 'app/Basecomponent/BasePopupAdd.component';
import { TranslocoService } from '@ngneat/transloco';
import { FuseConfirmationService } from '@fuse/services/confirmation';
import { ActivatedRoute } from '@angular/router';
import { NzTableQueryParams } from 'ng-zorro-antd/table';


@Component({
    selector: 'popup_log_detail',
    templateUrl: 'popup_detail.html',
})
export class popup_detailComponent {
    public filter: any;
    public pageSize = 10;
    public pageIndex = 1;
    public sortField: string;
    public total = 1;
    public listData: any;
    public pageLoading: Boolean = true;
    public currentIndex: number;
    constructor(public dialogRef: MatDialogRef<popup_detailComponent>,
       public http: HttpClient, _translocoService: TranslocoService,
        fuseConfirmationService: FuseConfirmationService, route: ActivatedRoute,
        @Inject('BASE_URL') baseUrl: string,
        @Inject(MAT_DIALOG_DATA) data: any) {
          this.filter=  { search: "",id_user_auth_log: data.id_user_auth_log,controller_name:""}
          
        // super(_translocoService, fuseConfirmationService, route, baseUrl, http, 'sys_mongodb_user_auth_log', dialogRef);

        // this.record = data;
        // this.Oldrecord = JSON.parse(JSON.stringify(data));

    }
    public rerender(): void {
        if(this.filter.search!=""&&this.filter.search.trim()!=""){
            this.pageSize = 10;
            this.pageIndex=1;
        }
        this.loadData(this.pageIndex, this.pageSize);
    };


    loadData(
        pageIndex: number,
        pageSize: number) {
        this.pageLoading = true;
        this.http
            .post('sys_mongodb_user_auth_log.ctr/DataHandlerDetail/',
                {
                    param1: { pageSize, pageIndex },
                    data: this.filter
                }
            ).subscribe((resp: any) => {
                console.log(resp);

                this.total = resp.recordsTotal;
                this.listData = resp.data.map((e:any)=>{
                    e.response_data= JSON.parse(e.response_data);
                    return e;
                });
                this.currentIndex = resp.start;
                this.pageLoading = false
            });
    }

    onQueryParamsChange(params: NzTableQueryParams): void {
        this.pageSize = params.pageSize;
        this.pageIndex = params.pageIndex;
        this.loadData(this.pageIndex, this.pageSize);
    }

}
