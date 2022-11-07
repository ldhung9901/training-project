import { Component, OnInit, ViewEncapsulation, Input, EventEmitter, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { TranslocoService } from '@ngneat/transloco';

@Component({
    selector     : 'cm_select',
    templateUrl  : './cm_select.component.html',
    styleUrls    : ['./cm_select.component.scss']
})
export class cm_selectComponent implements OnInit
{
    @Input() errorModel: any;
    @Input() keyError: string;
    @Input() label: string;
    @Input() type: string;
    @Input() paramCallBack: string;
    @Input() model: any;
    @Input() actionEnum: any = 1;
    @Input() listData: any;
    @Input() callbackChange: Function;
    @Input() callbackChangeWithParam: Function;
    @Input() callbackChangeSecond: Function;
    @Output() modelChange: EventEmitter<any> = new EventEmitter<any>();
    @Input() objectChose: any;
    @Output() objectChoseChange: EventEmitter<any> = new EventEmitter<any>();
    search: string = '';
    public selected: any;
    constructor(
        private _translocoService: TranslocoService,
    ) {

        if (this.type == '' || this.type == null) this.type = 'single';
    }

    ngOnInit() {




    }


    setChose(data): void {

            this.objectChose =[...this.listData] .filter(d => d.id == data)[0];
            this.objectChoseChange.emit(this.objectChose);
        if (this.callbackChange != undefined && this.callbackChange != null)
            this.callbackChange();

        if(this.callbackChangeSecond != undefined && this.callbackChangeSecond != null){
            this.callbackChangeSecond();
        }
        if (this.callbackChangeWithParam != undefined && this.callbackChangeWithParam != null)
            if (this.paramCallBack != undefined && this.paramCallBack != null) {
                this.callbackChangeWithParam(this.paramCallBack, this.model);
            } else {
                this.callbackChangeWithParam(this.label, this.model);
            }
    }








}

