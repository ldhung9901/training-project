import { Component, OnInit, ViewEncapsulation, Input, EventEmitter, Output } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ThemePalette } from '@angular/material/core';
import { NgxMatDateFormats } from '@angular-material-components/datetime-picker';
import { TranslocoService } from '@ngneat/transloco';
import moment from 'moment';
@Component({
    selector: 'cm_input',
    templateUrl: './cm_input.component.html',
    styleUrls: ['./cm_input.component.scss']
})
export class cm_inputComponent implements OnInit {

    @Input() maxlength: any;
    @Input() errorModel: any;
    @Input() keyError: string;
    @Input() type: string;
    @Input() min: number;
    @Input() attribute: string;
    @Input() label: string;
    @Input() labelAddString: string;
    @Input() suffixstring: string;
    @Input() placeholder: string;
    @Input() model: any;
    @Input() actionEnum: any = 1;
    @Input() callbackChange: Function;
    public id: any;
    @Output() modelChange: EventEmitter<any> = new EventEmitter<any>();
    myOptions: any;

    public disabled = false;
    public showSpinners = true;
    public showSeconds = false;
    public touchUi = false;
    public enableMeridian = false;
    public minDate: Date = null;
    public maxDate: Date = null;
    public stepHour = 1;
    public stepMinute = 1;
    public stepSecond = 1;
    public color: ThemePalette = 'accent';
    timemask = [/\d/, /\d/, ':', /\d/, /\d/];;
    public hide = true;
    constructor(
        private _translocoService: TranslocoService,
    ) {
        this.myOptions = {
            allowDecimalPadding: false


        }
        if (this.maxlength == '' || this.maxlength == null || this.maxlength == undefined) this.maxlength = 200;
        if (this.model == '' || this.model == null || this.model == undefined) this.model = "";
        if (this.label == 'so_phieu') this.model = this.model.trim();
        if (this.type == '' || this.type == null) this.type = 'text';

    }

    ngOnInit() {
        if (this.model == '' || this.model == null || this.model == undefined) this.model = "";
        if (this.maxlength == '' || this.maxlength == null || this.maxlength == undefined) this.maxlength = 200;
        if (this.type == '' || this.type == null) this.type = 'text';
        if (this.type == 'number' && (this.min == null || this.min == undefined)) this.min = 0;
    }

    setChose(): void {
        if (this.callbackChange != undefined && this.callbackChange != null)
            this.callbackChange();

    }
}

