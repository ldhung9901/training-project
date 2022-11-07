
export const DATE_FORMATS = {
    parse: {
        dateInput: 'LL'
    },
    display: {
        dateInput: 'DD-MM-YYYY',
        monthYearLabel: 'YYYY',
        dateA11yLabel: 'LL',
        monthYearA11yLabel: 'YYYY'
    }
};
import moment from "moment";
import { Inject, Injectable, Optional } from '@angular/core';
import { MomentDateAdapter } from '@angular/material-moment-adapter';
import { Moment } from 'moment';
import { MAT_DATE_LOCALE } from '@angular/material/core';

@Injectable()
export class MomentUtcDateAdapter extends MomentDateAdapter {

    constructor(@Optional() @Inject(MAT_DATE_LOCALE) dateLocale: string) {
        super(dateLocale);
    }

    createDate(year: number, month: number, date: number): Moment {
        // Moment.js will create an invalid date if any of the components are out of bounds, but we
        // explicitly check each case so we can throw more descriptive errors.
        if (month < 0 || month > 11) {
            throw Error(`Invalid month index "${month}". Month index has to be between 0 and 11.`);
        }

        if (date < 1) {
            throw Error(`Invalid date "${date}". Date has to be greater than 0.`);
        }

        let result = moment.utc({ year, month, date }).locale(this.locale);

        // If the result isn't valid, the date must have been out of bounds for this month.
        if (!result.isValid()) {
            throw Error(`Invalid date "${date}" for month with index "${month}".`);
        }

        return result;
    }
}


export const datepickerOptions= [
    {
        value: {
            start: null,
            end: null,
        },
        id:0,
        name: "Tùy chọn"
    },
    {
        value: {
            start: moment().toDate(),
            end: moment().toDate(),
        },
        id:1,
        name: "Hôm nay"
    },
    {
        value: {
            start: moment().subtract(1, "days").toDate(),
            end: moment().subtract(1, "days").toDate(),
        },
        id:2,
        name: "Hôm qua"
    },
    {
        value: {
            start: moment().subtract(7, "days").toDate(),
            end: moment().toDate(),
        },
        id:3,
        name: "7 Ngày"
    },
    {
        value: {
            start: moment().subtract(30, "days").toDate(),
            end: moment().toDate(),
        },
        id:4,
        name: "30 Ngày"
    },
    {
        value: {
            start: moment().startOf('month').toDate(),
            end: moment().endOf('month').toDate()
        },
        id:5,
        name: "Tháng này"
    },
    {
        value: {
            start: moment().subtract(1,'month').startOf('month').toDate(),
            end: moment().subtract(1,'month').endOf('month').toDate()
        },
        id:6,
        name: "Tháng trước"
    },
]
