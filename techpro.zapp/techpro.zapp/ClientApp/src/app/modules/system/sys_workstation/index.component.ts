import { Component, Inject, ViewChild } from '@angular/core';

import { HttpClient, HttpResponse } from '@angular/common/http';

import { DataTablesResponse } from 'app/Basecomponent/datatable';

import { TranslocoService } from '@ngneat/transloco';
import { sys_workstation_popUpAddComponent } from './popupAdd.component';
import { MatDialog } from '@angular/material/dialog';

import { Subject } from 'rxjs';
import { BaseIndexDatatableComponent } from 'app/Basecomponent/BaseIndexDatatable.component';

import { FuseConfirmationService } from '@fuse/services/confirmation';
import { ActivatedRoute } from '@angular/router';
import { CmSelectType } from '@fuse/components/commonComponent/cm_select_component/types';

@Component({
  selector: 'sys_workstation_index',
  templateUrl: './index.component.html',
  styleUrls: ['./index.component.scss'],
})
export class sys_workstation_indexComponent extends BaseIndexDatatableComponent {
  public listStatusDel: CmSelectType[];
  listphase: any;
  public listfactoryline: any;
  public listfactory: any;
  constructor(
    http: HttpClient,
    dialog: MatDialog,
    _translocoService: TranslocoService,
    fuseConfirmationService: FuseConfirmationService,
    route: ActivatedRoute,
    @Inject('BASE_URL') baseUrl: string
  ) {
    super(http, baseUrl, _translocoService, fuseConfirmationService, route, dialog, 'sys_workstation', {
      search: '',
      status_del: '1',
      id_sys_factory: '',
      id_sys_factory_line: '',
      id_sys_phase: '-1',
    });
    this.http.post('/sys_factory.ctr/getListUse/', {}).subscribe((resp) => {
      this.listfactory = resp;
    });
    this.http.post('/sys_phase.ctr/getListUse/', {}).subscribe((resp) => {
      this.listphase = resp;
      this.listphase.splice(0, 0, { id: "-1", name: this._translocoService.translate('all') })
    });

    this.listStatusDel = [
      {
        id: '1',
        name: this._translocoService.translate('system.use'),
      },
      {
        id: '2',
        name: this._translocoService.translate('system.not_use'),
      },
    ];
  }
  bind_data_factory(): void {
    this.filter.id_sys_factory_line = '';
    this.rerender();
    this.http
      .post('/sys_factory_line.ctr/getListUse/', {
        id_factory: this.filter.id_sys_factory,
      })
      .subscribe((resp) => {
        this.listfactoryline = resp;
      });
  }
  openDialogAdd(): void {
    // if (this.filter.id_sys_factory == '') {
    //   this.showMessageWarningNoConfirm('system.must_chose_factory');
    //   return;
    // }
    // if (this.filter.id_sys_factory_line == '') {
    //   this.showMessageWarningNoConfirm('system.must_chose_factory_line');
    //   return;
    // }

    const dialogRef = this.dialog.open(sys_workstation_popUpAddComponent, {
      disableClose: true,
      data: {
        actionEnum: 1,
        db: {
        //   id_sys_factory_line: this.filter.id_sys_factory_line,
        //   id_sys_factory: this.filter.id_sys_factory,
        //   id_sys_phase: this.filter.id_sys_phase,
          id: 0,
        },
      },
    });
    dialogRef.afterClosed().subscribe((result) => {
      if (result.db.id == 0) return;
      this.rerender();
    });
  }
  openDialogEdit(model, pos): void {
    model.id_sys_factory_line = this.filter.id_sys_factory_line;
    model.id_sys_factory = this.filter.id_sys_factory;
    model.id_sys_phase = this.filter.id_sys_phase;
    const dialogRef = this.dialog.open(sys_workstation_popUpAddComponent, {
      disableClose: true,
      data: model,
    });
    dialogRef.afterClosed().subscribe((result) => {
      if (result != undefined && result != null) this.listData[pos] = result;
      this.rerender();
    });
  }
  openDialogDetail(model, pos): void {
    model.id_sys_factory_line = this.filter.id_sys_factory_line;
    model.id_sys_factory = this.filter.id_sys_factory;
    model.id_sys_phase = this.filter.id_sys_factory;

    const dialogRef = this.dialog.open(sys_workstation_popUpAddComponent, {
      disableClose: true,
      data: model,
    });
    dialogRef.afterClosed().subscribe((result) => {
      if (result != undefined && result != null) this.listData[pos] = result;
    });
  }

  ngOnInit(): void {
    //this.baseInitData(this.pageIndex,this.pageSize,null,null,[]);
  }
}
