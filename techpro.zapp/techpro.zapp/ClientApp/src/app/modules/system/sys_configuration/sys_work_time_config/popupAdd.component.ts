import {
  Component,
  ElementRef,
  Inject,
  QueryList,
  Renderer2,
  TemplateRef,
  ViewChild,
  ViewChildren,
  ViewContainerRef,
} from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef, MatDialog } from '@angular/material/dialog';
import { HttpClient } from '@angular/common/http';
import { BasePopUpAddComponent } from 'app/Basecomponent/BasePopupAdd.component';
import { FuseConfirmationService } from '@fuse/services/confirmation';
import { ActivatedRoute } from '@angular/router';
import { TranslocoService } from '@ngneat/transloco';
import { ItemOtherUnit } from 'app/modules/system/sys_item/types';
import { Overlay, OverlayRef } from '@angular/cdk/overlay';
import { TemplatePortal } from '@angular/cdk/portal';
import _, { forIn } from 'lodash';
import { FilterPipe } from '@fuse/pipes/find-by-key/filter/filter.pipe';
import { DayBgRow } from '@fullcalendar/daygrid';
import { DIR_DOCUMENT_FACTORY } from '@angular/cdk/bidi/dir-document-token';
import moment, { Moment } from 'moment';
import { addDays, isThisSecond } from 'date-fns';
import { items } from 'app/mock-api/apps/file-manager/data';
import { actionEnums } from 'app/Basecomponent/actionEnums';
import { sys_working_time_detail_model, sys_working_time_model } from '../types';
import { ConsoleSqlOutline } from '@ant-design/icons-angular/icons';

@Component({
  selector: 'sys_working_time_config_popupAdd',
  templateUrl: 'popupAdd.html',
})
export class sys_working_time_config_popUpAddComponent extends BasePopUpAddComponent {
  record: sys_working_time_model;

  constructor(
    public dialog: MatDialog,
    public dialogRef: MatDialogRef<sys_working_time_config_popUpAddComponent>,
    http: HttpClient,
    _translocoService: TranslocoService,
    private _activatedRoute: ActivatedRoute,
    private _fuseConfirmationService: FuseConfirmationService,
    private _viewContainerRef: ViewContainerRef,
    fuseConfirmationService: FuseConfirmationService,
    route: ActivatedRoute,
    @Inject('BASE_URL') baseUrl: string,
    @Inject(MAT_DIALOG_DATA) data: sys_working_time_model
  ) {
    super(_translocoService, fuseConfirmationService, route, baseUrl, http, 'sys_configuration', dialogRef);

    this.record = { ...data };

    this.actionEnum = data.actionEnum;
    if (this.actionEnum == 1 || this.actionEnum == 2) {
        this.record.list_schedule = []
        this.baseInitData();
    }


    if (this.actionEnum == 3 || this.actionEnum == 2) {
        this.http
            .post('/sys_configuration.ctr/getListItem/', {
                id: this.record.db.id
            }
            ).subscribe((resp: sys_working_time_detail_model[]) => {
                this.record.list_schedule = resp;
            });

    }

  }

  save() {
      if(this.actionEnum === 1) {
          this.http
            .post('/sys_configuration.ctr/create_working_time/', {
              data: this.record,
            })
            .subscribe(
              (resp) => {
                  this.record = resp;
                  this.basedialogRef.close(this.record);
                  this.showMessageSuccessNoConfirm('save_success');

              },
              (error) => {
                if (error.status === 400) {
                  this.errorModel = error.error;
                }
              }
            );
      }else if(this.actionEnum ===2){
        this.http
                .post('/sys_configuration.ctr/edit_working_time/',
                    {
                        data: this.record,
                    }
                ).subscribe((resp) => {
                    this.record = resp;
                    this.Oldrecord = this.record;
                    this.basedialogRef.close(this.record);
                    this.showMessageSuccessNoConfirm('save_success');
                },
                    (error) => {
                        if (error.status === 400) {
                            this.errorModel = error.error;
                            this.aftersavefail();
                        }
                        this.loading = false;
                    });
      }
  }

  isYellow(item : sys_working_time_detail_model):boolean {

      return (moment(item.db.estimate_check_date).isoWeekday() ===7 ) || item.db.is_off;
  }
  generateSchedual() {

    this.record.list_schedule =[];

        let startDate = moment(this.record.db.schedual_date);
        let loopDate = startDate;
        let timeLoop = 0;

        // Condition : Loop until end year
        while (loopDate < moment(this.record.db.schedual_date).endOf('month')) {

            let obj = <sys_working_time_detail_model>{
                db: {
                    estimate_check_date: loopDate.toISOString(),
                    shift_1: 480,
                    shift_2: 480,
                    id_group: this.record.db.id,
                    is_off: loopDate.isoWeekday() ===7,
                }
            }
            loopDate = loopDate.add(1, 'd');
            this.record.list_schedule.push(obj);

            if (timeLoop > 100) {
                break;
            }

        }
        this.showMessageSuccessNoConfirm('maintenance.successGenerateSchedual');

}
}
