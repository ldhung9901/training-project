import { ChangeDetectorRef, Component, Inject, Input } from '@angular/core';

import { HttpClient } from '@angular/common/http';
import { TranslocoService } from '@ngneat/transloco';
import { MatDialog } from '@angular/material/dialog';
import { actionEnums } from 'app/Basecomponent/actionEnums';
import { FuseConfirmationService } from '@fuse/services/confirmation';
import { ActivatedRoute } from '@angular/router';
import { BaseIndexDatatableComponent } from 'app/Basecomponent/BaseIndexDatatable.component';
import { sys_configuration_model_v2 } from '../types';
import { CmSelectType } from '@fuse/components/commonComponent/cm_select_component/types';

@Component({
  selector: 'sys_format_license_config',
  templateUrl: './index.component.html',
})
export class sys_format_license_configComponent extends BaseIndexDatatableComponent {
  @Input() idSelect: string;
  editMode: boolean = false;
  public errorModel: any;

  listDataLicense: sys_configuration_model_v2[];
  public first_character?: string;
  public menu?: string;
  public is_raise?: boolean;
  list_recepcode: CmSelectType[];
  listMenu: CmSelectType[];
  constructor(
    public http: HttpClient,
    public dialog: MatDialog,
    public _translocoService: TranslocoService,
    fuseConfirmationService: FuseConfirmationService,
    private _changeDetectorRef: ChangeDetectorRef,
    route: ActivatedRoute,
    @Inject('BASE_URL') baseUrl: string
  ) {
    super(http, baseUrl, _translocoService, fuseConfirmationService, route, dialog, 'sys_configuration', {});
    this.http.post('/sys_configuration.ctr/get_List_license/', {}).subscribe((resp: sys_configuration_model_v2[]) => {
      this.listDataLicense = resp;
      this.list_recepcode = [
        {
          id: '1',
          name: '{P}{MM}{yy}/000',
        },
        {
          id: '2',
          name: '{PN}{MM}{yy}/0000',
        },
        {
          id: '3',
          name: '{PX}{MM}{yy}/00000',
        },
      ];
    });

    this.listMenu = [{
        id: 'Phiếu xuất',
        name: this._translocoService.translate("NAV.inventory_delivery")
    },
    {
        id: 'Phiếu nhâp',
        name: this._translocoService.translate("NAV.inventory_receiving")
    },
    {
        id: this._translocoService.translate("NAV.buyorder_purchase_order"),
        name: this._translocoService.translate("NAV.buyorder_purchase_order")
    },
    {
        id: this._translocoService.translate("NAV.business_sale_order"),
        name: this._translocoService.translate("NAV.business_sale_order")
    }
        ,
    {
        id: this._translocoService.translate("NAV.production_order"),
        name: this._translocoService.translate("NAV.production_order")
    }
        ,
    {
        id: this._translocoService.translate("NAV.maintenance_planning"),
        name: this._translocoService.translate("NAV.maintenance_planning")
    }
        ,
    {
        id: this._translocoService.translate("NAV.maintenance_process"),
        name: this._translocoService.translate("NAV.maintenance_process")
    }
        ,
    {
        id: this._translocoService.translate("NAV.maintenance_repair_process"),
        name: this._translocoService.translate("NAV.maintenance_repair_process")
    },
    {
        id: this._translocoService.translate("NAV.inventory_request_transfer"),
        name: this._translocoService.translate("NAV.inventory_request_transfer")
    }, {
        id: this._translocoService.translate("NAV.inventory_process_transfer"),
        name: this._translocoService.translate("NAV.inventory_process_transfer")

    }, {
        id: this._translocoService.translate("NAV.inventory_request_export"),
        name: this._translocoService.translate("NAV.inventory_request_export")

    }, {
        id: this._translocoService.translate("NAV.inventory_check_planning"),
        name: this._translocoService.translate("NAV.inventory_check_planning")
    },
    {
        id: this._translocoService.translate("NAV.inventory_check_process"),
        name: this._translocoService.translate("NAV.inventory_check_process")
    },
    {
        id: this._translocoService.translate("NAV.tool_write_increase"),
        name: this._translocoService.translate("NAV.tool_write_increase")
    },
    {
        id: this._translocoService.translate("NAV.tool_broken_voucher"),
        name: this._translocoService.translate("NAV.tool_broken_voucher")
    }]
  }

  /**
   * Toggle edit mode
   *
   * @param editMode
   */
  toggleEditMode(editMode: boolean | null = null): void {
    if (editMode === null) {
      this.editMode = !this.editMode;
    } else {
      this.editMode = editMode;
    }

    // Mark for check
    this._changeDetectorRef.markForCheck();
  }

  save() {
    this.http
      .post('/sys_configuration.ctr/create_license/', {
        data: {
          db_format_license_config: this.listDataLicense,
        },
      })
      .subscribe(
        (resp) => {
          this.showMessageSuccessNoConfirm('save_success');
          this.editMode = !this.editMode;
        },
        (error) => {
          if (error.status === 400) {
            this.errorModel = error.error;
          }
        }
      );
  }

  addNewLicenseList() {
    if (this.first_character == '' || this.first_character == null) {
      this.showMessageWarningNoConfirm('system.must_character_content');
      return;
    }
    if (this.menu == '' || this.menu == null) {
      this.showMessageWarningNoConfirm('system.must_choose_menu');
      return;
    }
    if (this.listDataLicense.filter((d) => d.menu == this.menu).length > 0) {
      this.showMessageWarningNoConfirm('system.menu_existed');
      return;
    }

    this.listDataLicense = [
      ...this.listDataLicense,
      {
        first_character: this.first_character,
        menu: this.menu,
        is_raise: this.is_raise,
        status_del: 1,
      },
    ];

    this.first_character = '';
    this.menu = '';
    this.is_raise = null;
  }
  deleteLicenseList(pos: number) {
    this.listDataLicense.splice(pos, 1);
    this.listDataLicense = [...this.listDataLicense];
  }
}
