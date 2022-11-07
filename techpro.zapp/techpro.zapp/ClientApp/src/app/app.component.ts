import { HttpClient } from '@angular/common/http';
import { Component, HostListener } from '@angular/core';


@Component({
    selector   : 'app-root',
    templateUrl: './app.component.html',
    styleUrls  : ['./app.component.scss']
})
export class AppComponent
{
    /**
     * Constructor
     */
    constructor(private  _httpClient: HttpClient)
    {

    }
    ngOnDestroy():void {

    }
    @HostListener('window:beforeunload', ['$event'])
   onWindowClose(event: any): void {

    // this._httpClient.get('users/logout',{
    //     params:
    //     {
    //         id:localStorage.getItem("log_id")?localStorage.getItem("log_id"):""
    //     }}
    //     )
    //     .subscribe((resp:any) => {
    //     });
    // localStorage.clear()
  }
}
