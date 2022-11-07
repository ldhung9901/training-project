import { Pipe, PipeTransform } from '@angular/core';

@Pipe({ name: 'currency_vnd' })
export class CurrencyPipe implements PipeTransform {
    transform(value: any): string {
        if (value) {
            let nbr = Number((value + '').replace(/,|-/g, ""));
            return (nbr + '').replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,");
        }
    }
}
