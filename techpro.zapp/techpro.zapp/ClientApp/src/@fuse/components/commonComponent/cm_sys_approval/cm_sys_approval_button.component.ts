import { Component, OnInit, ViewEncapsulation, Input, EventEmitter, Output, OnDestroy } from '@angular/core';
import { FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';
import Swal from 'sweetalert2';
import { BehaviorSubject, of, ReplaySubject, Subject } from 'rxjs';
import { filter, tap, takeUntil, debounceTime, map, delay, debounce, switchMap } from 'rxjs/operators';
import { HttpClient } from '@angular/common/http';
import { ProgressSpinnerMode } from '@angular/material/progress-spinner';
import { ThemePalette } from '@angular/material/core';
import { MatDialogRef, MatDialog } from '@angular/material/dialog';
import { cm_sys_approval_popupComponent } from './cm_sys_approval_popup.component';
import { FuseConfirmationService } from '@fuse/services/confirmation';
import { TranslocoService } from '@ngneat/transloco';
import { FuseNavigationItem, FuseNavigationService } from '@fuse/components/navigation';
import { compactNavigation, defaultNavigation, futuristicNavigation, horizontalNavigation } from 'app/mock-api/common/navigation/data';
import { InitialDataResolver, loadNavBar } from 'app/app.resolvers';
import { ActivatedRoute, Data, Router } from '@angular/router';
import { UserService } from 'app/core/user/user.service';

@Component({
    selector: 'cm_sys_approval_button',
    templateUrl: './cm_sys_approval_button.component.html'

})
export class cm_sys_approval_buttonComponent implements OnInit {
    @Input() id_sys_approval: any;
    @Input() id_record: any;
    @Input() create_by: any;
    @Input() create_date: any;
    @Input() menu: any;
    @Input() model: any;
    @Output() modelChange: EventEmitter<any> = new EventEmitter<any>();
    @Output() id_sys_approvalChange: EventEmitter<any> = new EventEmitter<any>();
    @Input() callbackChange: Function;
    navigation: any;
    public loading: boolean = true;
    public record: any = {
        db: {
            id_sys_approval_config: 0,
        }
    };

    public currentUser: any = JSON.parse(localStorage.getItem('user'));

    constructor(
        private _router: Router,
        private _activatedRoute: ActivatedRoute,
        private fuseConfirmationService: FuseConfirmationService,
        public dialogRef: MatDialogRef<cm_sys_approval_buttonComponent>,
        public _httpClient: HttpClient,
        public dialog: MatDialog,
        private route: ActivatedRoute,
        public _translocoService: TranslocoService,
    ) {

    }
    ngOnInit(): void {
        this.record.db.menu = this.menu;
        this.record.db.id_record = this.id_record;
        this.record.db.create_by_record = this.create_by;
        this.record.db.create_date_record = this.create_date;
        if (this.id_sys_approval == null) {
            this.id_sys_approval = 0;
            this.record.db.id = this.id_sys_approval;
        } else {
            this.record.db.id = this.model.id;
        }

        this.loading = false;
    }
    approval(): void {
        this.loading = true;
        this.record.db.status_action = 2;

        this._httpClient
            .post('sys_approval.ctr/approval/',
                this.record
            ).subscribe(resp => {
                this.loading = false;
                this.model = resp;
                this.modelChange.emit(this.model)

            });
    }
    cancel(): void {

        Swal.fire({
            title: this._translocoService.translate('common.reason'),
            input: 'text',
            inputAttributes: {
                autocapitalize: 'off'
            },
            showCancelButton: true,
            cancelButtonText: this._translocoService.translate('close'),
            confirmButtonText: this._translocoService.translate('common.confirm'),
            showLoaderOnConfirm: true,
            inputValidator: (value) => {
                if (!value) {
                    return this._translocoService.translate('common.must_input_reason')
                }
            },
            allowOutsideClick: () => false,
        }).then((result) => {
            if (result.value) {
                this.loading = true;
                this.record.db.status_action = 3;
                this.record.db.last_note = result.value;
                this._httpClient
                    .post('sys_approval.ctr/cancel/',
                        this.record
                    ).subscribe(resp => {
                        this.model = resp;
                        this.modelChange.emit(this.model);
                        this._httpClient
                            .post(this.record.db.menu + '.ctr/sync_cancel_approval/',
                                {
                                    id_approval: this.record.db.id,
                                    id_record: this.record.db.id_record
                                }
                            ).subscribe(resp => {
                                this.loading = false;
                            });

                    });
            }
        })
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
    close(): void {

        this._activatedRoute.data.pipe(
            switchMap((data: Data) => data.initialData)
        );
        Swal.fire({
            title: this._translocoService.translate('common.reason'),
            input: 'text',
            inputAttributes: {
                autocapitalize: 'off'
            },
            showCancelButton: true,
            cancelButtonText: this._translocoService.translate('close'),
            confirmButtonText: this._translocoService.translate('common.confirm'),
            showLoaderOnConfirm: true,
            inputValidator: (value) => {
                if (!value) {
                    return this._translocoService.translate('common.must_input_reason')
                }
            },
            allowOutsideClick: () => false,
        }).then((result) => {
            if (result.value) {
                this.loading = true;
                this.record.db.status_action = 6;
                this.record.db.last_note = result.value;
                this._httpClient
                    .post('sys_approval.ctr/close/',
                        this.record
                    ).subscribe(resp => {
                        this.loading = false;
                        this.model = resp;
                        this.modelChange.emit(this.model);

                    });
            }
        })

    }
    open(): void {

        Swal.fire({
            title: this._translocoService.translate('common.reason'),
            input: 'text',
            inputAttributes: {
                autocapitalize: 'off'
            },
            showCancelButton: true,
            cancelButtonText: this._translocoService.translate('close'),
            confirmButtonText: this._translocoService.translate('common.confirm'),
            showLoaderOnConfirm: true,
            inputValidator: (value) => {
                if (!value) {
                    return this._translocoService.translate('common.must_input_reason')
                }
            },
            allowOutsideClick: () => false,
        }).then((result) => {
            if (result.value) {
                this.loading = true;
                this.record.db.status_action = 5;
                this.record.db.last_note = result.value;
                this._httpClient
                    .post('sys_approval.ctr/open/',
                        this.record
                    ).subscribe(resp => {
                        this.model = resp;
                        this.modelChange.emit(this.model);
                        this._httpClient
                            .post(this.record.db.menu + '.ctr/sync_cancel_approval/',
                                {
                                    id_approval: this.record.db.id,
                                    id_record: this.record.db.id_record
                                }
                            ).subscribe(resp => {
                                this.loading = false;
                            });

                    });
            }
        })
    }
    return(): void {

        Swal.fire({
            title: this._translocoService.translate('common.reason'),
            input: 'text',
            inputAttributes: {
                autocapitalize: 'off'
            },
            showCancelButton: true,
            cancelButtonText: this._translocoService.translate('close'),
            confirmButtonText: this._translocoService.translate('common.return'),
            showLoaderOnConfirm: true,
            inputValidator: (value) => {
                if (!value) {
                    return this._translocoService.translate('common.must_input_reason')
                }
            },
            allowOutsideClick: () => false,
        }).then((result) => {
            if (result.value) {
                this.loading = true;
                this.record.db.status_action = 3;
                this.record.db.last_note = result.value;
                this._httpClient
                    .post('sys_approval.ctr/approval/',
                        this.record
                    ).subscribe(resp => {
                        this.loading = false;
                        this.model = resp;
                        this.modelChange.emit(this.model);

                    });
            }
        })

    }

    loadMenu(): void {
        let _compactNavigation: FuseNavigationItem[] = compactNavigation;
        let _defaultNavigation: FuseNavigationItem[] = defaultNavigation;
        let _futuristicNavigation: FuseNavigationItem[] = futuristicNavigation;
        let _horizontalNavigation: FuseNavigationItem[] = horizontalNavigation;
        this._httpClient.post('/sys_home.ctr/getModule/', {}).pipe(
            switchMap((resp: any) => {
                _defaultNavigation = loadNavBar(resp, this._translocoService);

                // Fill compact navigation children using the default navigation
                _compactNavigation.forEach((compactNavItem) => {
                    _defaultNavigation.forEach((defaultNavItem) => {
                        if (defaultNavItem.id === compactNavItem.id) {
                            compactNavItem.children = defaultNavItem.children;
                        }
                    });
                });
                // Fill futuristic navigation children using the default navigation
                _futuristicNavigation.forEach((futuristicNavItem) => {
                    _defaultNavigation.forEach((defaultNavItem) => {
                        if (defaultNavItem.id === futuristicNavItem.id) {
                            futuristicNavItem.children = defaultNavItem.children;
                        }
                    });
                });
                // Fill horizontal navigation children using the default navigation
                _horizontalNavigation.forEach((horizontalNavItem) => {
                    _defaultNavigation.forEach((defaultNavItem) => {
                        if (defaultNavItem.id === horizontalNavItem.id) {
                            horizontalNavItem.children = defaultNavItem.children;
                        }
                    });
                });
                // Return the response
                resp =
                {
                    compact: _compactNavigation,
                    default: _defaultNavigation,
                    futuristic: _futuristicNavigation,
                    horizontal: _horizontalNavigation
                };

                return of(resp);
            })
        ).subscribe()
    }
    openDialogApproval(): void {

        this.record.actionEnum = 1;
        const dialogRef = this.dialog.open(cm_sys_approval_popupComponent, {

            disableClose: true,
            data: this.record,
        });
        dialogRef.afterClosed().subscribe(result => {
            if (result) {
                if (this.id_sys_approval == 0) {
                    this.loading = true;
                    this._httpClient
                        .post(this.record.db.menu + '.ctr/sync_approval/',
                            {
                                id_approval: result?.id,
                                id_record: this.record.db.id_record
                            }
                        ).subscribe(resp => {
                            this.loading = false;
                        });
                }
                if (result != undefined && result != null) {
                    this.model = result;
                    this.record.db.id = result.id;
                    this.modelChange.emit(this.model)
                    this.id_sys_approvalChange.emit(this.id_sys_approval);

                    if (this.callbackChange != undefined && this.callbackChange != null)
                        this.callbackChange(result);
                }

            }

        });
    }
}

