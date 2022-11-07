import { Component, Inject, OnDestroy, OnInit, ViewEncapsulation } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, ValidationErrors, ValidatorFn, Validators } from '@angular/forms';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { fuseAnimations } from '@fuse/animations';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { HttpClient } from '@angular/common/http';

import { TranslocoService } from '@ngneat/transloco';
import { FuseNavigationService } from '@fuse/components/navigation';
import { ActivatedRoute } from '@angular/router';
import { sys_user_popUpAddComponent } from 'app/modules/system/sys_user/popupAdd.component';
import { FuseConfirmationService } from '@fuse/services/confirmation';


@Component({
    selector: 'popupChangePassword',
    templateUrl: './popupChangePassword.component.html',
    styleUrls: ['./popupChangePassword.component.scss'],
    encapsulation: ViewEncapsulation.None,
    animations: fuseAnimations
})
export class popupChangePasswordComponent implements OnInit, OnDestroy {
    resetPasswordForm: FormGroup;
    public hide: boolean= true;
    // Private
    private _unsubscribeAll: Subject<any>;

    constructor(
        public dialogRef: MatDialogRef<popupChangePasswordComponent>,
        public http: HttpClient,
        public fuseConfirmationService: FuseConfirmationService,
        public _translocoService: TranslocoService,
        private _formBuilder: FormBuilder,
        _fuseNavigationService: FuseNavigationService,
        route: ActivatedRoute,
        @Inject("BASE_URL") baseUrl: string,
        @Inject(MAT_DIALOG_DATA) data: any,
        public dialogModal: MatDialog,
    ){
        // Configure the layout
        dialogRef.disableClose = false;

        // Set the private defaults
        this._unsubscribeAll = new Subject();
    }
    public showMessageSuccessNoConfirm(msg: string): void {
        this.fuseConfirmationService.open({
            title: msg,
            message: '',
            icon: {
                name: 'heroicons_solid:check',
                color: 'success'
            },
            actions: {
                confirm: {
                    show: false
                }
            }

        }).afterClosed().subscribe((result) => {

        });
    }
    public showMessageWarningNoConfirm(msg: string): void {
        this.fuseConfirmationService.open({
            title: msg,
            message: '',
            actions: {
                confirm: {
                    show: false
                }
            }

        }).afterClosed().subscribe((result) => {

        });
    }
    public closeCustom(): void {
        this.dialogRef.close();
    }
    // -----------------------------------------------------------------------------------------------------
    // @ Lifecycle hooks
    // -----------------------------------------------------------------------------------------------------

    /**
     * On init
     */

    onNoClick(): void {
        this.dialogRef.close();
    }
    ngOnInit(): void {
        this.resetPasswordForm = this._formBuilder.group({

            oldPassword: ['', [Validators.required]],
            password: ['', Validators.required],
            passwordConfirm: ['', [Validators.required, confirmPasswordValidator]]
        });

        // Update the validity of the 'passwordConfirm' field
        // when the 'password' field changes
        this.resetPasswordForm.get('password').valueChanges
            .pipe(takeUntil(this._unsubscribeAll))
            .subscribe(() => {
                this.resetPasswordForm.get('passwordConfirm').updateValueAndValidity();
            });
    }

    /**
     * On destroy
     */
    ngOnDestroy(): void {
        // Unsubscribe from all subscriptions
        this._unsubscribeAll.next();
        this._unsubscribeAll.complete();
    }
    changePassword() {
        this.http
            .post('/users/changePassword/', {
                data: this.resetPasswordForm.value
            }
            ).subscribe(resp => {
               this.showMessageSuccessNoConfirm('Đổi mật khẩu thành công')
                    this.dialogRef.close();

            }, error => this.showMessageWarningNoConfirm("Mật khẩu cũ sai"));
    }
}




/**
 * Confirm password validator
 *
 * @param {AbstractControl} control
 * @returns {ValidationErrors | null}
 */
export const confirmPasswordValidator: ValidatorFn = (control: AbstractControl): ValidationErrors | null => {

    if (!control.parent || !control) {
        return null;
    }

    const password = control.parent.get('password');
    const passwordConfirm = control.parent.get('passwordConfirm');

    if (!password || !passwordConfirm) {
        return null;
    }

    if (passwordConfirm.value === '') {
        return null;
    }

    if (password.value === passwordConfirm.value) {
        return null;
    }

    return { passwordsNotMatching: true };
};



