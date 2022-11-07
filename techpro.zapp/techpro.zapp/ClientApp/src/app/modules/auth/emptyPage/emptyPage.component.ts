import { HttpClient } from '@angular/common/http';
import { Component, ViewEncapsulation } from '@angular/core';
import { fuseAnimations } from '@fuse/animations';

@Component({
    selector     : 'emptyPage',
    templateUrl  : './emptyPage.component.html',
    encapsulation: ViewEncapsulation.None,
})
export class EmptyPageComponent
{
    /**
     * Constructor
     */
    public data:any;
    constructor(public http: HttpClient)
    {
        this.http.post("users/all-report",{}).subscribe(resp =>{
            this.data = JSON.stringify(resp);
        }, error =>{
            this.data = JSON.stringify(error);
        }) ;
    }
}
