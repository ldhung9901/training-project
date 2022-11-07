import { Injectable, OnDestroy, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { Message } from 'app/layout/common/messages/messages.types';
import { map, switchMap, take } from 'rxjs/operators';
import * as signalR from "@microsoft/signalr";
import { User } from 'app/core/user/user.model';
import { NzNotificationPlacement, NzNotificationService } from 'ng-zorro-antd/notification';

@Injectable({
    providedIn: 'root'
})
export class MessagesService implements OnDestroy {
    private _messages: BehaviorSubject<Message[]> = new BehaviorSubject<Message[]>([]);
    private hubNotificationConnection: signalR.HubConnection;
    private user: User;
    /**
     * Constructor
     */
    constructor(private _httpClient: HttpClient, private notification: NzNotificationService) {
        this.user = JSON.parse(localStorage.getItem('user'));
        this.initSignalR();
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Accessors
    // -----------------------------------------------------------------------------------------------------

    /**
     * Getter for messages
     */
    get messages$(): Observable<Message[]> {
        return this._messages.asObservable();
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Public methods
    // -----------------------------------------------------------------------------------------------------

    /**
     * Store messages on the service
     *
     * @param messages
     */
    store(messages: Message[]): Observable<Message[]> {
        // Load the messages
        this._messages.next(messages);

        // Return the messages
        return this.messages$;
    }

    // /**
    //  * Create a message
    //  *
    //  * @param message
    //  */
    // create(message: Message): Observable<Message> {
    //     return this.messages$.pipe(
    //         take(1),
    //         switchMap(messages => this._httpClient.post<Message>('api/common/messages', { message }).pipe(
    //             map((newMessage) => {

    //                 // Update the messages with the new message
    //                 this._messages.next([...messages, newMessage]);

    //                 // Return the new message from observable
    //                 return newMessage;
    //             })
    //         ))
    //     );
    // }

    /**
     * Update the message
     *
     * @param id
     * @param message
     */
    update(id: string, message: Message): Observable<Message> {
        return this.messages$.pipe(
            take(1),
            switchMap(messages => this._httpClient.post<Message>('notification.ctr/update', message).pipe(
                map((updatedMessage: Message) => {

                    // Find the index of the updated message
                    const index = messages.findIndex(item => item.id === id);

                    // Update the message
                    messages[index] = updatedMessage;

                    // Update the messages
                    this._messages.next(messages);

                    // Return the updated message
                    return updatedMessage;
                })
            ))
        );
    }

    /**
     * Delete the message
     *
     * @param id
     */
    delete(id: string): Observable<boolean> {
        return this.messages$.pipe(
            take(1),
            switchMap(messages => this._httpClient.delete<boolean>('notification.ctr/delete', { params: { id } }).pipe(
                map(() => {

                    // // Find the index of the deleted message
                    // const index = messages.findIndex(item => item.id === id);

                    // // Delete the message
                    // messages.splice(index, 1);

                    // Update the messages
                    // this._messages.next(messages);

                    // Return the deleted status
                    return true;
                })
            ))
        );
    }

    /**
     * Mark all messages as read
     */
    markAllAsRead(): Observable<boolean> {
        return this.messages$.pipe(
            take(1),
            switchMap(messages => this._httpClient.get<boolean>('api/common/messages/mark-all-as-read').pipe(
                map((isUpdated: boolean) => {

                    // // Go through all messages and set them as read
                    // messages.forEach((message, index) => {
                    //     messages[index].is_read = true;
                    // });

                    // // Update the messages
                    // this._messages.next(messages);

                    // Return the updated status
                    return isUpdated;
                })
            ))
        );
    }
    public getAllNotifications(): void {
        this._httpClient.get<Message[]>('notification.ctr/get_all').subscribe(data => {
            this.store(data);
        });
    }
    async ngOnDestroy() {

        await this.hubNotificationConnection.invoke('outRoom', this.user.id)
            .then((resp) => { })
            .catch(err => { })
        await this.hubNotificationConnection.stop().then(() => { }).catch(err => console.error(err))
    }
    async initSignalR(): Promise<void> {

        this.hubNotificationConnection = new signalR.HubConnectionBuilder()
            .withUrl(`https://${window.location.host}/notification`)
            .withAutomaticReconnect()
            .configureLogging(signalR.LogLevel.None)
            .build();
        this.hubNotificationConnection
            .start()
            .then(() => { this.joinRoom(this.hubNotificationConnection, this.user.id) })
            .catch(err => console.error('Error while starting connection: ' + err));
        this.hubNotificationConnection.onreconnected((connectionId: any) => {
            console.warn('Reconnected ' + connectionId)
            this.joinRoom(this.hubNotificationConnection, this.user.id)
        });

        this.hubNotificationConnection.on('new_notification', (data: Message) => {
            this.createBasicNotification(data.title, data.content, data.type, 'topRight')
            this.getAllNotifications();
        });
        this.hubNotificationConnection.on('notification', (data: Message) => {
            this.getAllNotifications();
        });
    }
    private joinRoom(hubConnection: signalR.HubConnection, idRoom: string): void {
        hubConnection.invoke('joinRoom', idRoom)
            .then((resp) => { })
            .catch(err => { })
    }

    createBasicNotification(title: string = 'Bạn có thông báo mới !', content: string = '', type: string, place: NzNotificationPlacement): void {
        this.notification.create(
            type,
            title,
            content,
            { nzPlacement: place }
        );
    }

}
