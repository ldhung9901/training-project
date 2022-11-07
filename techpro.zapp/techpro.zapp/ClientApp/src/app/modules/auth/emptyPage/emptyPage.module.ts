import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';
import { FuseCardModule } from '@fuse/components/card';
import { SharedModule } from 'app/shared/shared.module';
import { EmptyPageComponent } from './emptyPage.component';
import { emptyPageRoutes } from './emptyPage.routing';

@NgModule({
    declarations: [
        EmptyPageComponent
    ],
    imports     : [
        RouterModule.forChild(emptyPageRoutes),
        MatButtonModule,
        FuseCardModule,
        SharedModule
    ]
})
export class emptyPageModule
{
}
