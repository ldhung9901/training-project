import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';


import { HttpClient } from '@angular/common/http';

import { TranslocoService } from '@ngneat/transloco';
import { BasePopUpAddComponent } from 'app/Basecomponent/BasePopupAdd.component';
import { FuseConfirmationService } from '@fuse/services/confirmation';
import { ActivatedRoute } from '@angular/router';
import { NzUploadChangeParam, NzUploadFile } from 'ng-zorro-antd/upload';
import { UserService } from 'app/core/user/user.service';



@Component({
    selector: 'sys_user_popupAdd',
    templateUrl: 'popupAdd.html',
})
export class sys_user_popUpAddComponent extends BasePopUpAddComponent {
    listdepartment: any;
    listjob_title: any;
    avatar_url: string;
    FileList: NzUploadFile[] = [];
    constructor(public dialogRef: MatDialogRef<sys_user_popUpAddComponent>,
        http: HttpClient, _translocoService: TranslocoService,
        public _userService: UserService,
        fuseConfirmationService: FuseConfirmationService, route: ActivatedRoute,
        @Inject('BASE_URL') baseUrl: string,
        @Inject(MAT_DIALOG_DATA) data: any) {
        super(_translocoService, fuseConfirmationService, route,baseUrl,http,'sys_user', dialogRef);
        this.record = data;
        this.Oldrecord = JSON.parse(JSON.stringify(data));
        this.avatar_url = this.record.db.avatar_path;
        this.http
            .post('/sys_department.ctr/getListUse/', {}
            ).subscribe(resp => {
                this.listdepartment = resp;
            });
        this.http
            .post('/sys_job_title.ctr/getListUse/', {}
            ).subscribe(resp => {
                this.listjob_title = resp;
            });
        this.actionEnum = data.actionEnum;
        if (this.actionEnum == 1) {
            this.baseInitData();
        }
    }

    handleChange(info: NzUploadChangeParam): void {
        if (info.file.status !== 'uploading') {


        }
        if (info.file.status === 'done') {
            console.log(info.fileList[0].response)
            let userInfo = JSON.parse(localStorage.getItem('user'));


            if (this.record.db.Id === userInfo['id']) {
                userInfo = { ...userInfo, ...info.fileList[0].response }
                this._userService.update(info.fileList[0].response);
                localStorage.setItem('user', JSON.stringify(userInfo));
            }

            this.record.db.avatar_path = info.fileList[0].response.avatar_path;
            this.avatar_url = this.record.db.avatar_path;
            console.log(this.avatar_url)
            this.FileList = [];
        } else if (info.file.status === 'error') {
            // this.msg.error(`${info.file.name} file upload failed.`);
        }
    }

}
