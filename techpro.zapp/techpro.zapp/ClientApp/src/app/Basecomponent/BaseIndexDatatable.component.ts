import { HttpClient } from '@angular/common/http';

import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { Component, ViewChild, Inject, Directive, OnDestroy, QueryList, ViewChildren, TemplateRef, ViewContainerRef } from '@angular/core';

import { Subject } from 'rxjs';

import { ActivatedRoute } from '@angular/router';
import { FuseConfirmationService } from '@fuse/services/confirmation';
import { TranslocoService } from '@ngneat/transloco';
import { DataTablesResponse } from 'app/Basecomponent/datatable';
import { clear } from 'console';
import { translateDataTable } from './VietNameseDataTable';
import { NzTableQueryParams } from 'ng-zorro-antd/table';
import { MatButton } from '@angular/material/button';
import { Overlay, OverlayRef } from '@angular/cdk/overlay';
import { TemplatePortal } from '@angular/cdk/portal';
import { FuseConfirmationDialogComponent } from '@fuse/services/confirmation/dialog/dialog.component';
import moment from 'moment';
import { datepickerOptions } from './config';


@Directive()
export abstract class BaseIndexDatatableComponent implements OnDestroy {
    @ViewChild('filterMorePanel') private _messagesPanel: TemplateRef<any>;
    @ViewChild('filterMoreOrigin') private _messagesOrigin: MatButton;

    public controller: String;
    public filter: any;
    public table: any;
    public exportBody: any;
    public receiptCode: string;
    public _overlayRef: OverlayRef;
    public pageLoading: Boolean = true;
    public currentIndex: number;
    public listData: any;
    public baseurl: String;
    public pageSize = 10;
    public pageIndex = 1;
    public sortField: string;
    public total = 1;
    public listSelectOptionDate: { id: number, value: { start: Date, end: Date }; name: string; }[] = datepickerOptions;

    constructor(public http: HttpClient,
        _baseUrl: string,
        public _translocoService: TranslocoService,
        public fuseConfirmationService: FuseConfirmationService,
        public route: ActivatedRoute,
        public dialog: MatDialog, _controller: String, _filter, private _overlay?: Overlay, private _viewContainerRef?: ViewContainerRef) {
        this.controller = _controller;
        this.baseurl = _baseUrl;
        this.filter = _filter;
        this.pageLoading = false;

    }
    ngOnDestroy(): void {

    }
    public rerender(): void {

        this.baseInitData(this.pageIndex, this.pageSize, null, null, []);
    };




    public formatSizeUnits(bytes) {
        if (bytes >= 1073741824) { bytes = (bytes / 1073741824).toFixed(2) + " GB"; }
        else if (bytes >= 1048576) { bytes = (bytes / 1048576).toFixed(2) + " MB"; }
        else if (bytes >= 1024) { bytes = (bytes / 1024).toFixed(2) + " KB"; }
        else if (bytes > 1) { bytes = bytes + " bytes"; }
        else if (bytes == 1) { bytes = bytes + " byte"; }
        else { bytes = "0 bytes"; }
        return bytes;
    }

    public rerenderfilter(): void {
        this.before_filter();
        this.baseInitData(this.pageIndex, this.pageSize, null, null, []);
    }
    public before_filter(): void {

    }
    _formatDatetime(date: Date, format: string): string {
        const _padStart = (value: number): string => value.toString().padStart(2, '0');
        return format
            .replace(/yyyy/g, _padStart(date.getFullYear()))
            .replace(/dd/g, _padStart(date.getDate()))
            .replace(/mm/g, _padStart(date.getMonth() + 1))
            .replace(/hh/g, _padStart(date.getHours()))
            .replace(/ii/g, _padStart(date.getMinutes()))
            .replace(/ss/g, _padStart(date.getSeconds()));
    }
    handleDataBefor(): void {
        if (this.total > 0 && this.listData.length == 0) {
            this.pageIndex = 1;
        }
    }


    public showMessageWarningNoConfirm(msg): void {

        this.fuseConfirmationService.open({
            title: msg,
            message: '',
            actions: {
                confirm: { show: false }
            }
        })

    }
    public showConfirmDialog(msg: string = 'savethechanges?'): MatDialogRef<FuseConfirmationDialogComponent, any> {
        return this.fuseConfirmationService.open({
            title: msg,
            message: '',
        });
    }
    public showMessageSuccessNoConfirm(msg): void {

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
                    .post(this.controller + '.ctr/delete/',
                        {
                            id: id1,
                        }
                    ).subscribe(resp => {
                        this.rerender();
                    });
            }
        });


    }

    baseInitData(
        pageIndex: number,
        pageSize: number,
        sortField: string | null,
        sortOrder: string | null,
        filter: Array<{ key: string; value: string[] }>): void {
        this.pageLoading = true;
        this.http
            .post(this.baseurl + '' + this.controller + '.ctr/DataHandler/',
                {
                    param1: { pageSize, pageIndex, sortField, sortOrder, filter },
                    data: this.filter
                }
            ).subscribe((resp: any) => {
                this.total = resp.recordsTotal;
                this.receiptCode = resp.receiptCode;
                this.listData = resp.data;
                this.currentIndex = resp.start;
                this.handleDataBefor();
                this.pageLoading = false
            });
    }
    onQueryParamsChange(params: NzTableQueryParams): void {
        this.pageSize = params.pageSize;
        this.pageIndex = params.pageIndex;
        const filter = params.filter;
        const sort = params.sort;
        const currentSort = sort.find(item => item.value !== null);
        const sortField = (currentSort && currentSort.key) || null;
        const sortOrder = (currentSort && currentSort.value) || null;
        this.baseInitData(this.pageIndex, this.pageSize, sortField, sortOrder, filter);
    }
    public _createOverlay(): void {
        // Create the overlay
        this._overlayRef = this._overlay.create({
            hasBackdrop: true,
            backdropClass: 'fuse-backdrop-on-mobile',
            scrollStrategy: this._overlay.scrollStrategies.block(),
            positionStrategy: this._overlay.position()
                .flexibleConnectedTo(this._messagesOrigin._elementRef.nativeElement)
                .withLockedPosition(true)
                .withPush(true)
                .withPositions([
                    {
                        originX: 'start',
                        originY: 'bottom',
                        overlayX: 'start',
                        overlayY: 'top'
                    },
                    {
                        originX: 'start',
                        originY: 'top',
                        overlayX: 'start',
                        overlayY: 'bottom'
                    },
                    {
                        originX: 'end',
                        originY: 'bottom',
                        overlayX: 'end',
                        overlayY: 'top'
                    },
                    {
                        originX: 'end',
                        originY: 'top',
                        overlayX: 'end',
                        overlayY: 'bottom'
                    }
                ])
        });

        // Detach the overlay from the portal on backdrop click
        this._overlayRef.backdropClick().subscribe(() => {
            this._overlayRef.detach();
        });
    }
    ////////////////////////////////////////////////////////////
    openPanel(): void {
        // Return if the messages panel or its origin is not defined
        if (!this._messagesPanel || !this._messagesOrigin) {
            return;
        }

        // Create the overlay if it doesn't exist
        if (!this._overlayRef) {
            this._createOverlay();
        }

        // Attach the portal to the overlay
        this._overlayRef.attach(new TemplatePortal(this._messagesPanel, this._viewContainerRef));
    }

    closePanel(): void {
        this._overlayRef.detach();
    }

    closePanelAfterFilter() {
        this.rerender();
        this.closePanel();
    }
    ////////////////////////////////////////////////////////////
    // DateRangePicker
    bindingSelectOptionDate(type: 'select' | 'datepicker'): void {
        if (type === 'datepicker') {
            this.filter.range.type = [...this.listSelectOptionDate].find(e =>

                (moment(this.filter.range.start).format('DD-MM-YYYY') === moment(e.value.start).format('DD-MM-YYYY')) &&
                (moment(this.filter.range.end).format('DD-MM-YYYY') === moment(e.value.end).format('DD-MM-YYYY'))

            )?.id || [...this.listSelectOptionDate][0].id;
        }
        else if (type === 'select') {
            const findedItem = this.listSelectOptionDate.find(e => e.id == this.filter.range.type);
            this.filter.range = { ...findedItem.value, type: findedItem.id };
        }
    }
    ////////////////////////////////////////////////////////////

    trackByFn(index: number, item: any): any {
        return item.id || index;
    }
}
