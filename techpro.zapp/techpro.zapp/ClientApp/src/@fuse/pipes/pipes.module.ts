import { NgModule } from '@angular/core';
import { CurrencyPipe } from './currencypipe/currencypipe';

import { FilterPipe } from './find-by-key/filter/filter.pipe';
import { FilterCorrectPipe } from './find-by-key/filter/filterCorrect.pipe';
import { safeHtml, SafePipe } from './safe.pipe';


@NgModule({
    declarations: [

        FilterPipe,
        FilterCorrectPipe,
        SafePipe,
        safeHtml,
        CurrencyPipe

    ],
    imports     : [],
    exports     : [
        SafePipe,
        FilterCorrectPipe,
        FilterPipe,
        safeHtml,
        CurrencyPipe

    ]
})
export class FusePipesModule
{
}
