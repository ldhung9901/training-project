import { NgModule } from '@angular/core';
import { commonModule } from './common.module';

import { MatButtonModule } from '@angular/material/button';
import { MatChipsModule } from '@angular/material/chips';
import { MatNativeDateModule, MatRippleModule } from '@angular/material/core';
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
import { cm_sys_approval_popupComponent } from './cm_sys_approval/cm_sys_approval_popup.component';
import { cm_sys_approval_statusComponent } from './cm_sys_approval/cm_sys_approval_status.component';
import { cm_sys_approval_status_popupComponent } from './cm_sys_approval/cm_sys_approval_status_popup.component';
import { cm_sys_approval_filterComponent } from './cm_sys_approval/cm_sys_approval_filter.component';
import { cm_sys_approval_buttonComponent } from './cm_sys_approval/cm_sys_approval_button.component';
import { TranslocoModule } from '@ngneat/transloco';
import { FusePipesModule } from '@fuse/pipes/pipes.module';
import { CommonModule } from '@angular/common';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NzSelectModule } from 'ng-zorro-antd/select';
import { NgxMatDatetimePickerModule, NgxMatNativeDateModule, NgxMatTimepickerModule } from '@angular-material-components/datetime-picker';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { MatCardModule } from '@angular/material/card';
import { NzTableModule } from 'ng-zorro-antd/table';
import { FuseConfirmationModule } from '@fuse/services/confirmation';

@NgModule({
    declarations: [
        cm_sys_approval_filterComponent,
        cm_sys_approval_buttonComponent,
        cm_sys_approval_popupComponent,
        cm_sys_approval_statusComponent,
        cm_sys_approval_status_popupComponent,
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
        commonModule,
        MatCardModule,
        NzTableModule,
        FuseConfirmationModule
    ],
    exports: [
        cm_sys_approval_filterComponent,
        cm_sys_approval_buttonComponent,
        cm_sys_approval_popupComponent,
        cm_sys_approval_statusComponent,
        cm_sys_approval_status_popupComponent,

    ]
})

export class ApprovalModule {
}
