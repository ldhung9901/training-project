import { Component, Directive, Inject, ViewChild } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ActivatedRoute } from '@angular/router';
import { TranslocoService } from '@ngneat/transloco';
import { FuseConfirmationService } from '@fuse/services/confirmation';



@Directive()
export abstract class BasePopUpAddComponent {
    parserInt = value => value.replace('.', '').replace(/[^0-9]/g, '');
    formatter = (value) => `${value}`.replace(/\B(?=(\d{3})+(?!\d))/g, ',');
    public Oldrecord: any;
    public record: any;
    public errorModel: any;
    public actionEnum: number;
    public baseurl: string;
    public loading = false;
    constructor(
        public _translocoService: TranslocoService,
        public fuseConfirmationService: FuseConfirmationService,
        public route: ActivatedRoute,
        _baseUrl: string,
        public http: HttpClient,
        public controller: string,
        public basedialogRef: any,


    ) {
        this.errorModel = [];
        this.baseurl = _baseUrl;
    }
    public showMessagewarning(msg): void {

        this.fuseConfirmationService.open({
            title: msg,
            message: '',

        })

    }
    public beforesave(): boolean {
        return true;

    }
    public aftersavefail(): void {


    }
    public aftersave(): void {


    }
    public closeCustom(): void {

        if (this.actionEnum == 3) {
            this.close();
        }
        else {
            this.fuseConfirmationService.open({
                title: 'savethechanges?',
                message: '',
                icon: {
                    name: 'heroicons_outline:clipboard-list',
                    color: 'primary'
                },
                actions: {
                    confirm: {
                        color: 'primary',
                        label: 'save'
                    }
                }

            }).afterClosed().subscribe((result) => {

                // If the confirm button pressed...
                if (result === 'confirmed') {
                    this.save()
                }
                else {
                    this.close();
                }
            });
        }

    }
    public showMessageSuccessNoConfirm(msg: string,typeMsg:number=0): void {
        this.fuseConfirmationService.open({
            title: msg,
            message: '',
            typeMsg:typeMsg,//nếu type bằng 0 thì không cần phải dùng _translocoService khi truyền vào
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
    public showMessageWarningNoConfirm(msg: string,typeMsg:number=0): void {
        this.fuseConfirmationService.open({
            title: msg,
            message: '',
            typeMsg:typeMsg,
            actions: {
                confirm: {
                    show: false
                }
            }

        }).afterClosed().subscribe((result) => {

        });
    }
    public baseInitData(): void {
        this.save();
    };
    save(): void {


        if (this.beforesave() === false) {
            return;
        }

        this.loading = true;
        if (this.actionEnum === 1) {
            this.http
                .post(this.controller + '.ctr/create/',
                    {
                        data: this.record,
                    }
                ).subscribe((resp) => {
                    this.record = resp;
                    this.Oldrecord = this.record;
                    this.loading = false;
                    this.basedialogRef.close(this.record);
                    this.fuseConfirmationService.open({
                        title: 'save_success',
                        message: '',
                        actions: {
                            confirm: { show: false }
                        },
                        icon: {
                            name: 'heroicons_solid:check',
                            color: 'success'
                        }
                    })


                    this.aftersave();
                },
                    (error) => {
                        if (error.status === 400) {
                            this.errorModel = error.error;
                            this.aftersavefail();
                        }
                        this.loading = false;

                    }
                );
        } else if (this.actionEnum === 2) {
            this.http
                .post(this.controller + '.ctr/edit/',
                    {
                        data: this.record,
                    }
                ).subscribe((resp) => {
                    this.record = resp;
                    this.Oldrecord = this.record;
                    this.basedialogRef.close(this.record);
                    this.fuseConfirmationService.open({
                        title: 'save_success',
                        message: '',
                        actions: {
                            confirm: { show: false }
                        },
                        icon: {
                            name: 'heroicons_solid:check',
                            color: 'success'
                        }
                    })
                    this.aftersave();

                },
                    (error) => {
                        if (error.status === 400) {
                            this.errorModel = error.error;
                            this.aftersavefail();
                        }
                        this.loading = false;

                    });
        } else if (this.actionEnum === 4) {
            this.http
                .post(this.controller + '.ctr/copy/',
                    {
                        data: this.record,
                    }
                ).subscribe((resp) => {

                    this.record = resp;

                    this.basedialogRef.close(this.record);
                    this.aftersave();
                },
                    (error) => {
                        if (error.status === 400) {
                            this.errorModel = error.error;
                        }
                        this.loading = false;

                    });
        }
        else { this.loading = false };
    }

    close(): void {
        if (this.actionEnum === 3) {
            this.basedialogRef.close(this.record);
        } else {
            this.basedialogRef.close(this.Oldrecord);
        }

    }

    public formatSizeUnits(bytes) {
        if (bytes >= 1073741824) { bytes = (bytes / 1073741824).toFixed(2) + " GB"; }
        else if (bytes >= 1048576) { bytes = (bytes / 1048576).toFixed(2) + " MB"; }
        else if (bytes >= 1024) { bytes = (bytes / 1024).toFixed(2) + " KB"; }
        else if (bytes > 1) { bytes = bytes + " bytes"; }
        else if (bytes == 1) { bytes = bytes + " byte"; }
        else { bytes = "0 bytes"; }
        return bytes;
    }
    trackByFn(index: number, item: any): any {
        return item.id || index;
    }

}
