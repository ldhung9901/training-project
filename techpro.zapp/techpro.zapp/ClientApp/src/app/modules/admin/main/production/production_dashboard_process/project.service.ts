import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ActivatedRouteSnapshot, Resolve, RouterStateSnapshot } from '@angular/router';
import { Observable } from 'rxjs';
import * as signalR from "@microsoft/signalr";
import { workstationMachineInfomation } from './IWorkstationInfomationType';
import { productionProcessCount } from './IProductionProcessCount';

@Injectable()
export class ProductionProcessDashboardService implements Resolve<any>
{
    filter: any = { search: "", id_workstation: "", workstation_name: "", status_del: "1", working_date: new Date().toISOString(), id_item: "" };
    projects: any[];
    gaugeChart: any;
    widgets: any[];
    allQuantityNeedProduce: number = 0;
    domain: string = window.location.host;
    workstationMachineInfomation: workstationMachineInfomation = { line_speed: 0, oil_level: 0, pressure: 0, temperatureDevice: 0, temperatureFurnace: 0, sampleTime: 1 };
    productionProcessCount: productionProcessCount = { numErr: 0, numOrder: 0, numPro: 0, progress: 0 };
    hubConnection: signalR.HubConnection;
    list_workstation: Array<any>;
    expirationCounter: number = 1;
    counter: any =null;
    /**
     * Constructor
     *
     * @param {HttpClient} _httpClient
     */
    constructor(
        private _httpClient: HttpClient
    ) {
        this.gaugeChart = {
            legendTitle: "",
            single: [{
                name: "Nhiệt độ của máy",
                value: 2
            }],
            legend: false,
            legendPosition: 'below',
            colorScheme: {
                domain: ['midnightblue', '#E44D25', '#CFC0BB', '#7aa3e5', '#a8385d', '#aae3f5']
            }
        }


    }

    /**
     * Resolver
     *
     * @param {ActivatedRouteSnapshot} route
     * @param {RouterStateSnapshot} state
     * @returns {Observable<any> | Promise<any> | any}
     */
    resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<any> | Promise<any> | any {

        return new Promise((resolve, reject) => {

            Promise.all([
                this.getProjects(),
                this.getListWorkStation()
            ]).then(
                () => {
                    resolve(null);
                },
                reject
            );
        });
    }

    /**
     * Get projects
     *
     * @returns {Promise<any>}
     */
    getProjects(): Promise<any> {
        return new Promise((resolve, reject) => {
            this._httpClient.post('/production_dashboard_process.ctr/projects/', {})
                .subscribe((response: any) => {
                    this.projects = response;
                    resolve(resolve)
                }, reject);
        });
    }
    getListWorkStation(): Promise<any> {
        return new Promise((resolve, reject) => {
            this._httpClient.post("/sys_workstation.ctr/getListUse/", {}).subscribe((res: Array<any>) => {
                this.list_workstation = res;
                this.filter.id_workstation = res[0].id;
                this.filter.workstation_name = res[0].name;
                resolve(resolve);
            })
            // ;
        });
    }
    public updateTemporaryHistory(id_process: number, quantity_product: number, quantity_error: number): void {
        this.hubConnection.invoke('updateTemporaryHistory', id_process, quantity_product, quantity_error)
            .then((resp) => { console.log('updateTemporaryHistory:' + resp); })
            .catch(err => console.error('updateTemporaryHistory: ' + err))
    }

    public startConnection = (id_workstation: string) => {
        this.hubConnection = new signalR.HubConnectionBuilder()
            .withUrl(`https://${this.domain}/production_dashboard_process/workStationInfo`)
            .withAutomaticReconnect()
            .configureLogging(signalR.LogLevel.Information)
            .build();
        this.hubConnection
            .start()
            .then(() => { console.log('Connection started'); this.joinRoom(this.filter.id_workstation); this.reset_timer() })
            .catch(err => console.error('Error while starting connection: ' + err))
    }

    public stopConnection = () => {
        console.log('stop ...')

        this.hubConnection.off('machineInfomation');
        this.hubConnection.off('productionProcess');

        this.hubConnection.stop().then(() => console.log('Connection stopped')).catch(err => console.error('error error errorerror error'))

    }
    public changeRoom(old_id_workstation: string, new_id_workstation: string): void {
        this.hubConnection.invoke('outRoom', old_id_workstation)
            .then((resp) => { console.log('outRoom ' + resp); this.joinRoom(new_id_workstation) })
            .catch(err => console.error('Error outRoom room: ' + err))
    }

    public joinRoom(id_workstation: string): void {
        this.hubConnection.invoke('joinRoom', id_workstation)
            .then((resp) => {
                console.log('joinRoom ' + resp);

            })
            .catch(err => console.error('Error join room: ' + err))
    }

    // reset count and timer
    public reset_timer(): void {
        clearInterval(this.counter);
        this.expirationCounter = this.workstationMachineInfomation.sampleTime;
        this.counter = setInterval(this.timer.bind(this), 1000);
    }



    public timer(): void {

        this.expirationCounter--;
        if (this.expirationCounter <= 0) {
            clearInterval(this.counter);
            this.reset_timer();
            return;
        }
    }
    public addTransferChartDataListener = () => {
        this.hubConnection.on('productionProcess', (data: productionProcessCount) => {
            this.productionProcessCount = data;
            Math.round((data.numPro / this.allQuantityNeedProduce * 100) * 100) / 100
            this.productionProcessCount.progress = Math.round((data.numPro / this.allQuantityNeedProduce * 100) * 100) / 100;
        });
        this.hubConnection.on('machineInfomation', (data) => {
            this.gaugeChart = {
                ...this.gaugeChart, single: [{
                    name: "Nhiệt độ của máy",
                    value: data.temperatureDevice
                }]
            };
            this.workstationMachineInfomation = data;
            this.reset_timer();
        });
    }
}
