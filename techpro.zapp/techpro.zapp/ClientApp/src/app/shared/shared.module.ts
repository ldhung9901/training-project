import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NzEmptyModule } from 'ng-zorro-antd/empty';
import { registerLocaleData } from '@angular/common';
import vi from '@angular/common/locales/vi';
import * as AllIcons from '@ant-design/icons-angular/icons';
import { NzTypographyModule } from 'ng-zorro-antd/typography';
registerLocaleData(vi);
import { NZ_I18N, vi_VN } from 'ng-zorro-antd/i18n';
import { commonModule } from '@fuse/components/commonComponent/common.module';
import { ApprovalModule } from '@fuse/components/commonComponent/approval.module';
import { NzSpinModule } from 'ng-zorro-antd/spin';
import { NzSkeletonModule } from 'ng-zorro-antd/skeleton';
import { NzTagModule } from 'ng-zorro-antd/tag';
import { NzPopoverModule } from 'ng-zorro-antd/popover';
import { NzCheckboxModule } from 'ng-zorro-antd/checkbox';
import { FuseCardModule } from '@fuse/components/card';
import { NzProgressModule } from 'ng-zorro-antd/progress';
import { NzSliderModule } from 'ng-zorro-antd/slider';
import { NzLayoutModule } from 'ng-zorro-antd/layout';
import { NzGridModule } from 'ng-zorro-antd/grid';
import { NzSelectModule } from 'ng-zorro-antd/select';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzInputNumberModule } from 'ng-zorro-antd/input-number';
import { NzListModule } from 'ng-zorro-antd/list';
import { NzTableModule } from 'ng-zorro-antd/table';
import { NzUploadModule } from 'ng-zorro-antd/upload';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzMessageModule } from 'ng-zorro-antd/message';
import { NzNotificationModule } from 'ng-zorro-antd/notification';
import { TranslocoModule } from '@ngneat/transloco';
import { NzImageModule } from 'ng-zorro-antd/image';
import { FusePipesModule } from '@fuse/pipes/pipes.module';
import { NzAvatarModule } from 'ng-zorro-antd/avatar';
import { ScrollingModule } from '@angular/cdk/scrolling';
import { NzPopconfirmModule } from 'ng-zorro-antd/popconfirm';
import { NzDatePickerModule } from 'ng-zorro-antd/date-picker';
import { NzModalModule } from 'ng-zorro-antd/modal';
import { NzCardModule } from 'ng-zorro-antd/card';
import { NzPaginationModule } from 'ng-zorro-antd/pagination';
import { NzSwitchModule } from 'ng-zorro-antd/switch';
import { NzDividerModule } from 'ng-zorro-antd/divider';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatCardModule } from '@angular/material/card';
import { MatInputModule } from '@angular/material/input';
import { NzAffixModule } from 'ng-zorro-antd/affix';
import { NzRadioModule } from 'ng-zorro-antd/radio';
import { IconDefinition } from '@ant-design/icons-angular';
import { NzIconModule, NZ_ICONS } from 'ng-zorro-antd/icon';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { CalendarModule } from '@fuse/components/commonComponent/calendar/calendar.module';
import { MatSlideToggle, MatSlideToggleModule } from '@angular/material/slide-toggle';
import { FuseConfirmationModule } from '@fuse/services/confirmation';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatMenuModule } from '@angular/material/menu';
import { MatSelectModule } from '@angular/material/select';
import { NgxMatSelectSearchModule } from 'ngx-mat-select-search';
import { MatButtonToggleModule } from '@angular/material/button-toggle';
import { MatTabsModule } from '@angular/material/tabs';
import { QuillModule } from 'ngx-quill';

registerLocaleData(vi);
const antDesignIcons = AllIcons as {
    [key: string]: IconDefinition;
};
const icons: IconDefinition[] = Object.keys(antDesignIcons).map(key => antDesignIcons[key])
@NgModule({
    imports: [

    ],
    providers   : [
        { provide: NZ_I18N, useValue: vi_VN },
        { provide: NZ_ICONS, useValue: icons },
      ],
    exports: [
        CommonModule,
        FormsModule,
        ReactiveFormsModule,
        NzEmptyModule,
        ApprovalModule,
        NzSpinModule,
        NzSkeletonModule,
        NzListModule,
        NzTagModule,
        NzEmptyModule,
        NzPopoverModule,
        NzCheckboxModule,
        FuseCardModule,
        NzProgressModule,
        NzSliderModule,
        NzLayoutModule,
        NzGridModule,
        NzInputModule,
        NzInputNumberModule,
        NzSelectModule,
        NzUploadModule,
        NzButtonModule,
        NzMessageModule,
        NzNotificationModule,
        TranslocoModule,
        NzImageModule,
        FusePipesModule,
        NzAvatarModule,
        ScrollingModule,
        NzModalModule ,
        NzPopconfirmModule,
        NzDatePickerModule,
        NzTypographyModule,
        NzCardModule,
        NzPaginationModule,
        NzSwitchModule,
        NzDividerModule,
        MatFormFieldModule,
        MatCardModule,
        MatInputModule,
        NzAffixModule,
        NzRadioModule,
        NzTableModule,
        MatProgressBarModule,
        NzIconModule,
        MatSlideToggleModule,
        FuseConfirmationModule,
        MatIconModule,
        MatButtonModule,
        MatMenuModule,
        MatSelectModule,
        NgxMatSelectSearchModule,
        MatButtonToggleModule,
        MatTabsModule,

    ]
})
export class SharedModule
{
}
