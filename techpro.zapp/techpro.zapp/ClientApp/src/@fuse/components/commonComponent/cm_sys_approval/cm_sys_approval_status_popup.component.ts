import { Component, OnInit, ViewEncapsulation, Input, EventEmitter, Output, OnDestroy, Inject, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';

import { ReplaySubject, Subject } from 'rxjs';
import { filter, tap, takeUntil, debounceTime, map, delay } from 'rxjs/operators';
import { HttpClient } from '@angular/common/http';
import { ProgressSpinnerMode } from '@angular/material/progress-spinner';
import { ThemePalette } from '@angular/material/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

import { ActivatedRoute } from '@angular/router';
import { FuseConfirmationService } from '@fuse/services/confirmation';
import { MatTableDataSource } from '@angular/material/table';

@Component({
    selector: 'cm_sys_approval_status_popup',
    templateUrl: './cm_sys_approval_status_popup.component.html'
})
export class cm_sys_approval_status_popupComponent {
    public record: any;
    public loading: any = true;
    icon = [
        'heroicons_solid:star', // Bat dau quy trinh duyet
        'heroicons_solid:check-circle', // Da duyet
        'heroicons_solid:refresh', // Tra ve
        'heroicons_solid:x-circle', // Huy phieu
        'heroicons_solid:lock-open', // Mo phieu
        'heroicons_solid:lock-closed', // Dong phieu
    ];
    //table
    dataTableColumns: string[] = ['step_num', 'step_name', 'user_name', 'duration_hours', 'note'];
    dataSource = new MatTableDataSource<any>([]);


    constructor(public dialogRef: MatDialogRef<cm_sys_approval_status_popupComponent>,
        public http: HttpClient,
        public route: ActivatedRoute,
        fuseConfirmationService: FuseConfirmationService,
        @Inject('BASE_URL') baseUrl: string,
        @Inject(MAT_DIALOG_DATA) data: any) {
        this.record = data;
        this.http
            .post('/sys_approval.ctr/getElementById/',
                {
                    id: this.record.id
                }
            ).subscribe((resp) => {
                this.record = resp;
            });
        this.loading = false;

    }

    close(): void {
        this.dialogRef.close();
    }

}


