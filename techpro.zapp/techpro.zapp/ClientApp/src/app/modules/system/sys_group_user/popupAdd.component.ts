import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';


import { HttpClient } from '@angular/common/http';
import { BasePopUpAddComponent } from 'app/Basecomponent/BasePopupAdd.component';
import { TranslocoService } from '@ngneat/transloco';
import { FuseConfirmationService } from '@fuse/services/confirmation';
import { ActivatedRoute } from '@angular/router';
import { actionEnums } from 'app/Basecomponent/actionEnums';



@Component({
    selector: 'sys_group_user_popupAdd',
    templateUrl: 'popupAdd.html',
})
export class sys_group_user_popUpAddComponent extends BasePopUpAddComponent {
    public additem: any;
    public additemrole: any;
    public item_chose: any;

    public searchRole: any='';
    public listRoleFilter: any=[];
    constructor(public dialogRef: MatDialogRef<sys_group_user_popUpAddComponent>,
        http: HttpClient, _translocoService: TranslocoService,
        fuseConfirmationService: FuseConfirmationService, route: ActivatedRoute,
        @Inject('BASE_URL') baseUrl: string,
        @Inject(MAT_DIALOG_DATA) data: any) {
        super(_translocoService, fuseConfirmationService, route,baseUrl,http,'sys_group_user', dialogRef);
        this.record = data;
        this.Oldrecord = JSON.parse(JSON.stringify(data));
        this.resetAddItem();
        this.resetAddItemRole();

        this.actionEnum = data.actionEnum;


            this.http
                .post('/sys_group_user.ctr/getListItem/', {
                    id: this.record.db.id
                }
                ).subscribe(resp => {
                    this.record.list_item = resp;
                });
            this.http
                .post('/sys_group_user.ctr/getListRole/', {
                    id: this.record.db.id
                }
                ).subscribe(resp => {
                    this.record.list_role = resp;
                    if (this.actionEnum != actionEnums.Detail) {
                        this.http
                            .post('/sys_home.ctr/getListRoleFull/', {}
                            ).subscribe(resp => {
                                this.listRoleFilter = resp;
                                this.resetlistRole();
                            });
                    }
                });


        if (this.actionEnum == 1) {
            this.baseInitData();
        }



    }

    resetAddItem(): void {
        this.additem = {
            db: {
                user_id: null,
                note: "",

            },
            user_name: "",
        };

    }
    resetAddItemRole(): void {
        this.additemrole = {
            db: {
                id_controller_role: "",
                controller_name: "",
                role_name: "",

            },
        };

    }
    bind_data_item_chose(): void {
        this.additem.db.user_id = this.item_chose.id;
        this.additem.user_name = this.item_chose.name;
    }
    addDetail(): void {
        var valid = true;
        var error = '';

        if (this.record.list_item.filter(d => d.db.user_id == this.additem.db.user_id ).length > 0) {
            error += this._translocoService.translate('existed');
            valid = false;
        }

        if ( this.additem.db.user_id == null || this.additem.db.user_id == undefined) {
            error += this._translocoService.translate('must_chose_item');
            valid = false;
        }

        if (!valid) {
            this.showMessageWarningNoConfirm(error);
            return;
        }
        this.record.list_item = [... this.record.list_item,this.additem]
        this.record.list_item.sort(function (a, b) {
            return a.db.step_num - b.db.step_num;
        });

        this.resetAddItem();
    }
    beforesave(): boolean {
        this.record.list_role = [];
        for (var i = 0; i < this.listRoleFilter.length; i++) {

            var d = this.listRoleFilter[i];
            if (this.listRoleFilter[i].completed == true) {
                this.record.list_role.push({
                    db: {
                        id_controller_role: d.role.id,
                        controller_name: d.controller_name,
                        role_name: d.role.name,
                    },

                });
            }
        }
        return true;


    }
    aftersavefail(): void {

    }
    resetlistRole(): void {
        for (var i = 0; i < this.listRoleFilter.length; i++) {
            this.listRoleFilter[i].name = this._translocoService.translate(this.listRoleFilter[i].controller_name) + " " + this._translocoService.translate(this.listRoleFilter[i].role.name)
            if (this.record.list_role.filter(d => d.db.id_controller_role == this.listRoleFilter[i].role.id).length > 0) {

                this.listRoleFilter[i].completed = true;
            } else {
                this.listRoleFilter[i].completed = false;
            }
        }
        this.updateAllComplete();


    }
    allComplete: boolean = false;

    updateAllComplete() {
        this.allComplete = this.listRoleFilter != null && this.listRoleFilter.every(t => t.completed);
    }

    someComplete(): boolean {
        if (this.listRoleFilter == null) {
            return false;
        }
        return this.listRoleFilter.filter(t => t.completed).length > 0 && !this.allComplete;
    }

    setAll(completed: boolean) {
        this.allComplete = completed;
        if (this.listRoleFilter == null) {
            return;
        }
        this.listRoleFilter.forEach(t => t.completed = completed);
    }

    deleteDetail(pos): void {
        this.record.list_item.splice(pos, 1);
        this.record.list_item = [... this.record.list_item]
    }

}
