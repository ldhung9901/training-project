import { LOCALE_ID, NgModule } from '@angular/core';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ExtraOptions, PreloadAllModules, RouterModule } from '@angular/router';
import { MarkdownModule } from 'ngx-markdown';
import { FuseModule } from '@fuse';
import { FuseConfigModule } from '@fuse/services/config';
import { FuseMockApiModule } from '@fuse/lib/mock-api';
import { CoreModule } from 'app/core/core.module';
import { appConfig } from 'app/core/config/app.config';
import { mockApiServices } from 'app/mock-api';
import { LayoutModule } from 'app/layout/layout.module';
import { AppComponent } from 'app/app.component';
import { appRoutes } from 'app/app.routing';
import { commonModule } from '@fuse/components/commonComponent/common.module';
import { ApprovalModule } from '@fuse/components/commonComponent/approval.module';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { DatePipe } from '@angular/common';
import { DateAdapter, MAT_DATE_FORMATS, MAT_DATE_LOCALE } from '@angular/material/core';
import { DATE_FORMATS, MomentUtcDateAdapter } from './Basecomponent/config';
import { BrowserModule } from '@angular/platform-browser';
import { AuthInterceptor } from './core/auth/auth.interceptor';
import es from '@angular/common/locales/es';
import { registerLocaleData } from '@angular/common';
import { DragDropModule } from '@angular/cdk/drag-drop';
import { QuillModule } from 'ngx-quill';

registerLocaleData(es);

export function getBaseUrl() {
    return document.getElementsByTagName('base')[0].href;
}
const providers = [

    { provide: 'BASE_URL', useFactory: getBaseUrl, deps: [] },
    { provide: LOCALE_ID, useValue: "vi-VI" },
];
const routerConfig: ExtraOptions = {
    scrollPositionRestoration: 'enabled',
    preloadingStrategy: PreloadAllModules
};
@NgModule({
    providers: [
        DatePipe,
        providers,
        { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true },
        { provide: DateAdapter, useClass: MomentUtcDateAdapter, deps: [MAT_DATE_LOCALE] },
        { provide: MAT_DATE_FORMATS, useValue: DATE_FORMATS }
    ],
    declarations: [
        AppComponent
    ],
    imports: [
        BrowserAnimationsModule,
        RouterModule.forRoot(appRoutes, routerConfig),
        HttpClientModule,
        // Fuse, FuseConfig & FuseMockAPI
        FuseModule,
        FuseConfigModule.forRoot(appConfig),
        FuseMockApiModule.forRoot(mockApiServices),
        DragDropModule,

        // Core module of your application
        CoreModule,

        // Layout module of your application
        LayoutModule,

        // 3rd party modules that require global configuration via forRoot
        MarkdownModule.forRoot({}),


    ],
    bootstrap: [
        AppComponent
    ]
})
export class AppModule {
}
