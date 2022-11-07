import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef, MatDialog } from '@angular/material/dialog';


import { HttpClient } from '@angular/common/http';

import { TranslocoService } from '@ngneat/transloco';
import { BasePopUpAddComponent } from 'app/Basecomponent/BasePopupAdd.component';
import { FuseConfirmationService } from '@fuse/services/confirmation';
import { ActivatedRoute } from '@angular/router';


@Component({
    selector: 'sys_help_page_detail',
    templateUrl: 'detail.html',
})
export class sys_help_page_detailComponent extends BasePopUpAddComponent {
    record: any;
    id:any;
    drawerMode: 'over' | 'side' = 'side';
    drawerOpened: boolean = true;

    /**
     * Constructor
     */


    public plugintiny = [
        "advlist autolink lists link image charmap print preview anchor",
        "searchreplace visualblocks code fullscreen",
        "insertdatetime media table paste imagetools wordcount"
    ];
    public toolbartiny = "insertfile undo redo | styleselect | bold italic | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | link image | preview";

    public timyconfig = {
        base_url: '/tinymce'
        , suffix: '.min',
        height: 500,
        images_upload_url: '/FileManager/tiny_mce_upload',
        plugins: this.plugintiny,
        toolbar: this.toolbartiny
    }
    constructor(public dialogRef: MatDialogRef<sys_help_page_detailComponent>,
        http: HttpClient, _translocoService: TranslocoService,
        fuseConfirmationService: FuseConfirmationService, route: ActivatedRoute,
        @Inject('BASE_URL') baseUrl: string,
        public dialogModal: MatDialog,
        @Inject(MAT_DIALOG_DATA) data: any) {
        super(_translocoService, fuseConfirmationService, route, baseUrl, http, 'sys_help_detail', dialogRef);
        this.id = data;
        this.Oldrecord = JSON.parse(JSON.stringify(data));
        this.actionEnum = data.actionEnum;

    }
    ngOnInit(): void
    {
        console.log(this.id.db);

        // this.route.params.subscribe( params => this.id = params.id);
        const url = 'sys_help_detail.ctr/getElementById?id=' + this.id.db.id;

        this.http.get(url).subscribe(
            (resp) => {
                this.record = resp;
            }
        );

    }

}
