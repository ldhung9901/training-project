import { ChangeDetectorRef, Component, Inject, Input } from '@angular/core';

import { HttpClient } from '@angular/common/http';
import { TranslocoService } from '@ngneat/transloco';
import { MatDialog } from '@angular/material/dialog';
import { actionEnums } from 'app/Basecomponent/actionEnums';
import { FuseConfirmationService } from '@fuse/services/confirmation';
import { ActivatedRoute } from '@angular/router';
import { BaseIndexDatatableComponent } from 'app/Basecomponent/BaseIndexDatatable.component';
import { sys_configuration_model } from '../types';

@Component({
  selector: 'sys_company_info',
  templateUrl: './index.component.html',
})
export class sys_company_infoComponent extends BaseIndexDatatableComponent{
  @Input() idSelect: string;
  datacompany: sys_configuration_model;
  editMode: boolean = false;
  public errorModel: any;

  constructor(
    public http: HttpClient,
    public dialog: MatDialog,
    public _translocoService: TranslocoService,
    fuseConfirmationService: FuseConfirmationService,
    private _changeDetectorRef: ChangeDetectorRef,
    route: ActivatedRoute,
    @Inject('BASE_URL') baseUrl: string
  ) {super(http, baseUrl, _translocoService, fuseConfirmationService, route, dialog,'sys_configuration',
  { }
)
    this.http
    .get('/sys_configuration.ctr/getElementById/', {
        params: { id: "1" },
    })
    .subscribe((resp: sys_configuration_model) => {
      this.datacompany = resp;
    });

  }

  /**
     * Toggle edit mode
     *
     * @param editMode
     */
   toggleEditMode(editMode: boolean | null = null): void
   {
       if ( editMode === null )
       {
           this.editMode = !this.editMode;
       }
       else
       {
           this.editMode = editMode;
       }

       // Mark for check
       this._changeDetectorRef.markForCheck();
   }

   save() {
            this.http
                .post('/sys_configuration.ctr/edit_company/',
                    {
                        data: {
                            db_company: {
                                id: this.datacompany.db_company.id,
                                name: this.datacompany.db_company.name,
                                address: this.datacompany.db_company.address,
                                phone: this.datacompany.db_company.phone,
                                tax_number: this.datacompany.db_company.tax_number,
                                user_service: this.datacompany.db_company.user_service,
                                pathimg: this.datacompany.db_company.pathimg,
                                status_del: 1,
                            }
                    }
                }).subscribe(resp => {
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

    selectFile(event): void {

        if (event.target.files.length > 0) {
            if (event.target.files[0].type.split("/")[0] == "image") {
                var formData = new FormData();
                formData.append('folder', 'sys_configuration');
                formData.append('file', event.target.files[0]);

                this.http
                    .post("fileManager.ctr/upload_file",
                        formData
                    ).subscribe((resp) => {
                        this.datacompany.db_company.pathimg = resp.toString();
                    },

                        error => {
                            if (error.status == 400) {
                                this.errorModel = error.error;

                            }
                        }
                    );
            }
            else {
                this.showMessageWarningNoConfirm('system.please_choose_picture')
            }
        }

    }
    removeItemImg() {
        // console.log(this.datacompany.db_company.pathimg)
        this.datacompany.db_company.pathimg = null;

    }


}
