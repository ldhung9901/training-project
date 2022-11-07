import { NgModule } from '@angular/core';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzSelectModule } from 'ng-zorro-antd/select';

import { MatButtonModule } from '@angular/material/button';
import { MatChipsModule } from '@angular/material/chips';
import { MatNativeDateModule, MAT_DATE_LOCALE } from '@angular/material/core';
import { MatRippleModule } from '@angular/material/core';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatSelectModule } from '@angular/material/select';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { MatSortModule } from '@angular/material/sort';
import { MatTableModule } from '@angular/material/table';
import { MatTabsModule } from '@angular/material/tabs';
import { MatDialogModule } from '@angular/material/dialog';
import { MatMenuModule } from '@angular/material/menu';
import { SweetAlert2Module } from '@sweetalert2/ngx-sweetalert2';
import { NgxMatSelectSearchModule } from 'ngx-mat-select-search';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { NgxMatDateAdapter, NgxMatDatetimePickerModule, NgxMatNativeDateModule, NgxMatTimepickerModule, NGX_MAT_DATE_FORMATS } from '@angular-material-components/datetime-picker';
import { NgxMatMomentModule, NGX_MAT_MOMENT_DATE_ADAPTER_OPTIONS } from '@angular-material-components/moment-adapter';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CustomNgxDatetimeAdapter, CUSTOM_DATE_FORMATS } from './CustomNgxDatetimeAdapter';
import { cm_select_server_sideComponent } from './cm_select_server_side_component/cm_select_server_side.component';
import { cm_selectComponent } from './cm_select_component/cm_select.component';
import { TranslocoModule } from '@ngneat/transloco';
import { FusePipesModule } from '@fuse/pipes/pipes.module';
import { CommonModule } from '@angular/common';
import { cm_inputComponent } from './cm_input_component/cm_input.component';
import { SharedModule } from 'app/shared/shared.module';
import { NzTableModule } from 'ng-zorro-antd/table';

@NgModule({
    providers: [
        {
            provide: NgxMatDateAdapter,
            useClass: CustomNgxDatetimeAdapter,
            deps: [MAT_DATE_LOCALE, NGX_MAT_MOMENT_DATE_ADAPTER_OPTIONS]
        },
        { provide: NGX_MAT_DATE_FORMATS, useValue: CUSTOM_DATE_FORMATS }
    ],
    declarations: [
        cm_select_server_sideComponent,
        cm_selectComponent,
        cm_inputComponent,
    ],
    imports: [
        SweetAlert2Module.forRoot(),
        MatChipsModule,
        MatExpansionModule,
        NzButtonModule,
        MatIconModule,
        MatInputModule,
        MatMenuModule,
        MatPaginatorModule,
        MatRippleModule,
        MatSelectModule,
        MatCheckboxModule,
        MatSortModule,
        MatSnackBarModule,
        MatTableModule,
        MatTabsModule,
        MatDialogModule,
        NzSelectModule,
        MatProgressSpinnerModule,
        NgxMatSelectSearchModule,
        NgxMatDatetimePickerModule,
        NgxMatTimepickerModule,
        FormsModule,
        ReactiveFormsModule,
        MatNativeDateModule,
        MatDatepickerModule,
        MatButtonModule,
        MatFormFieldModule,
        NgxMatNativeDateModule,
        TranslocoModule,
        FusePipesModule,
        CommonModule,


    ],
    exports: [
        cm_select_server_sideComponent,
        cm_selectComponent,
        cm_inputComponent,
    ]
})

export class commonModule {
}
