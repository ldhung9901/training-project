import { Component, OnInit, ViewEncapsulation, Input, EventEmitter, Output } from '@angular/core';
import { TranslocoService } from '@ngneat/transloco';


@Component({
    selector     : 'cm_sys_approval_filter',
    templateUrl  : './cm_sys_approval_filter.component.html'
})
export class cm_sys_approval_filterComponent implements OnInit
{

    @Input() label: string;
    @Input() model: any;
    @Input() callbackChange: Function;
    @Output() modelChange: EventEmitter<any> = new EventEmitter<any>();
    public listData: any;
    search: string ='';
    constructor(
        public _translocoService: TranslocoService,
    ) {
    }

    ngOnInit() {
        this.listData = [
            {
                id: -1,
                name: this._translocoService.translate("common.all")
            },
            {
                id: 1,
                name: this._translocoService.translate("common.create")
            },
            {
                id: 2,
                name: this._translocoService.translate("common.waiting_approval")
            },
            {
                id: 3,
                name: this._translocoService.translate("common.approved")
            },
            {
                id: 4,
                name: this._translocoService.translate("common.cancel")
            },
            {
                id: 5,
                name: this._translocoService.translate("common.return")
            },
            {
                id: 6,
                name: this._translocoService.translate("common.waiting_me")
            },
            {
                id: 7,
                name: this._translocoService.translate("common.return_for_me")
            },
             {
                id: 8,
                name: this._translocoService.translate("common.record_closed")
            }

        ]
    }
    setChose(data): void {
        console.log(this.callbackChange)
        if (this.callbackChange != undefined && this.callbackChange != null)
        this.callbackChange();
    }









}

