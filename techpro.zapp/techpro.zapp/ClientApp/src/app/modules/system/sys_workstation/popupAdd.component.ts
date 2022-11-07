import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';

import { HttpClient } from '@angular/common/http';
import { BasePopUpAddComponent } from 'app/Basecomponent/BasePopupAdd.component';
import { TranslocoService } from '@ngneat/transloco';
import { FuseConfirmationService } from '@fuse/services/confirmation';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'sys_workstation_popupAdd',
  templateUrl: 'popupAdd.html',
})
export class sys_workstation_popUpAddComponent extends BasePopUpAddComponent {
  listsys_opc: any;
  listphase: any;
  public listfactoryline: any;
  public listfactory: any;
  constructor(
    public dialogRef: MatDialogRef<sys_workstation_popUpAddComponent>,
    http: HttpClient,
    _translocoService: TranslocoService,
    fuseConfirmationService: FuseConfirmationService,
    route: ActivatedRoute,
    @Inject('BASE_URL') baseUrl: string,
    @Inject(MAT_DIALOG_DATA) data: any
  ) {
    super(_translocoService, fuseConfirmationService, route, baseUrl, http, 'sys_workstation', dialogRef);
    this.record = data;
    this.Oldrecord = JSON.parse(JSON.stringify(data));
    this.actionEnum = data.actionEnum;
    if (this.actionEnum != 3) {
    }
    if (this.actionEnum == 1) {
      this.baseInitData();
    }
    this.http.post('/sys_factory.ctr/getListUse/', {}).subscribe((resp) => {
      this.listfactory = resp;
    });
    this.http.post('/sys_phase.ctr/getListUse/', {}).subscribe((resp) => {
      this.listphase = resp;
    });
  }

  bind_data_factory(): void {
    this.record.id_sys_factory_line = '';
    this.http
      .post('/sys_factory_line.ctr/getListUse/', {
        id_factory: this.record.db.id_sys_factory,
      })
      .subscribe((resp) => {
        this.listfactoryline = resp;
      });
  }

  selectFile(event): void {
    if (event.target.files.length > 0) {
      if (event.target.files[0].type.split('/')[0] == 'image') {
        var formData = new FormData();
        formData.append('folder', this.controller);
        formData.append('file', event.target.files[0]);

        this.http.post('fileManager.ctr/upload_file', formData).subscribe(
          (resp) => {
            this.record.db.pathimg = resp.toString();
          },
          (error) => {
            if (error.status == 400) {
              this.errorModel = error.error;
            }
          }
        );
      } else {
        this.showMessageWarningNoConfirm('system.please_choose_picture');
      }
    }
  }

  removeItemImg() {
    this.record.db.pathimg = null;
}

changeStatus(event) {
    if (event.checked) {
        this.record.db.status_del = 1;
    }
    if (!event.checked) {
        this.record.db.status_del = 2;
    }
}
}
