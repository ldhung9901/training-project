import { Component, Inject, ViewChild } from '@angular/core';

import { HttpClient, HttpResponse } from '@angular/common/http';

import { sys_working_time_config_popUpAddComponent } from './popupAdd.component';
import { MatDialog } from '@angular/material/dialog';

import { Subject } from 'rxjs';
import { BaseIndexDatatableComponent } from 'app/Basecomponent/BaseIndexDatatable.component';

import { FuseConfirmationService } from '@fuse/services/confirmation';
import { ActivatedRoute } from '@angular/router';
import { ThemePalette } from '@angular/material/core';
import { ProgressSpinnerMode } from '@angular/material/progress-spinner';
import { TranslocoService } from '@ngneat/transloco';
import { actionEnums } from 'app/Basecomponent/actionEnums';

import { common_history_log_form_popUpComponent } from 'app/modules/system/sys_mongodb_user_auth_log/common_history_log_form_popup';
import moment from 'moment';
import { sys_working_time_model } from '../types';
import { id } from '@swimlane/ngx-charts';

@Component({
  selector: 'sys_working_time_config',
  templateUrl: './index.component.html',
})
export class sys_working_time_config_indexComponent extends BaseIndexDatatableComponent {
  listData: sys_working_time_model[];
  listworkingtime:sys_working_time_model[];

  constructor(
    http: HttpClient,
    dialog: MatDialog,
    _translocoService: TranslocoService,
    fuseConfirmationService: FuseConfirmationService,
    route: ActivatedRoute,
    @Inject('BASE_URL') baseUrl: string
  ) {
    super(http, baseUrl, _translocoService, fuseConfirmationService, route, dialog, 'sys_configuration', {});

    this.http
      .post('/sys_configuration.ctr/get_List_working_time/', {

      })
      .subscribe((resp: sys_working_time_model[]) => {
        this.listworkingtime = resp;
      });
  }

  openDialogAdd(): void {
    const dialogRef = this.dialog.open(sys_working_time_config_popUpAddComponent, {
      disableClose: true,
      data: <sys_working_time_model>{
        actionEnum: 1,
        db: {
          id: null,
        //   schedual_date: moment().toISOString(),
        },
        list_schedule: [],
      },
    });
    dialogRef.afterClosed().subscribe((result) => {
      if (result.db.id == 0) return;
      this.rerender();
    });
  }
  openDialogDetail(model: sys_working_time_model, pos: number): void {
    model.actionEnum = actionEnums.Detail;
    model.list_schedule = [];
    const dialogRef = this.dialog.open(sys_working_time_config_popUpAddComponent, {
        disableClose: true,
        data: model,

    });
    dialogRef.afterClosed().subscribe((result: sys_working_time_model) => {
        if (result != undefined && result != null) this.listworkingtime[pos] = result;
    });
}
openDialogEdit(model: sys_working_time_model, pos: number): void {
    model.actionEnum = actionEnums.Edit;
    const dialogRef = this.dialog.open(sys_working_time_config_popUpAddComponent, {
        disableClose: true,
        data: model
    });
    dialogRef.afterClosed().subscribe((result: sys_working_time_model) => {
        if (result != undefined && result != null) this.listData[pos] = result;
    });
}

public delete(id1): void {
    this.fuseConfirmationService.open({
        title: 'areYouSure',
        message: '',
        icon: {
            name: 'heroicons_outline:trash',
            color: 'error'
        }
    }).afterClosed().subscribe((result) => {

        // If the confirm button pressed...
        if (result === 'confirmed') {
            this.http
                .post('/sys_configuration.ctr/delete_working_time/',
                    {
                        id: id1,
                    }
                ).subscribe(resp => {
                    this.rerender();
                });
        }
    });

}

  ngOnInit(): void {
    //this.baseInitData(this.pageIndex,this.pageSize,null,null,[]);
  }
}
