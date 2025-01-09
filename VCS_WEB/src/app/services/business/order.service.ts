import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { BehaviorSubject, Observable } from 'rxjs';
import { OrderModel } from '../../models/bussiness/order.model';
import { CommonService } from '../common.service';
import * as signalR from '@aspnet/signalr';
import { filter } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class OrderService {
  private hubUrl = `${environment.baseApiUrl.replace('/api', '')}/order`;
  private hubConnection!: signalR.HubConnection;
  private notificationSubject = new BehaviorSubject<OrderModel | null>(null);
  private notificationReadSubject = new BehaviorSubject<string | null>(null);
  private notificationReadAllSubject = new BehaviorSubject<boolean>(false);
  private notificationsSubject = new BehaviorSubject<{ data: OrderModel[], hasMore: boolean }>({
    data: [],
    hasMore: true
  });
  private loadingSubject = new BehaviorSubject<boolean>(false);
  private unreadCountSubject = new BehaviorSubject<number>(0);
  constructor(private commonService: CommonService) { }

  public async initializeConnection(): Promise<void> {
    try {
      if (this.hubConnection) {
        await this.hubConnection.stop();
      }

      const token = localStorage.getItem('token') || 'null';
      const options: signalR.IHttpConnectionOptions = {
        accessTokenFactory: () => token,
        transport: signalR.HttpTransportType.WebSockets
      };

      this.hubConnection = new signalR.HubConnectionBuilder()
        .withUrl(this.hubUrl, {
          skipNegotiation: true,
          transport: signalR.HttpTransportType.WebSockets
        })
        .build();
      console.log('SignalR Connection Initialized');

      await this.hubConnection.start();
      this.registerSignalREvents();
    } catch (error) {
      console.error('SignalR Connection Error:', error);
    }
  }

  private registerSignalREvents(): void {
    this.hubConnection.on('NOTIFICATION_SEND', (notification: OrderModel) => {
      const currentNotifications = this.notificationsSubject.value.data;
      this.notificationsSubject.next({
        data: [notification, ...currentNotifications],
        hasMore: true
      });
      const currentUnreadCount = this.unreadCountSubject.value;
      this.unreadCountSubject.next(currentUnreadCount + 1);
    });

    this.hubConnection.on('NOTIFICATION_READ', (notificationId: string) => {
      const currentNotifications = this.notificationsSubject.value.data;
      const updatedNotifications = currentNotifications.map(n =>
        n.id === notificationId ? { ...n, isRead: true } : n
      );
      this.notificationsSubject.next({
        data: updatedNotifications,
        hasMore: true
      });
      const currentUnreadCount = this.unreadCountSubject.value;
      this.unreadCountSubject.next(Math.max(0, currentUnreadCount - 1));
    });

    this.hubConnection.on('NOTIFICATION_READ_ALL', () => {
      const currentNotifications = this.notificationsSubject.value.data;
      const updatedNotifications = currentNotifications.map(n => ({ ...n, isRead: true }));
      this.notificationsSubject.next({
        data: updatedNotifications,
        hasMore: true
      });
      this.unreadCountSubject.next(0);
    });
  }

  public onNotification(): Observable<OrderModel> {
    return this.notificationSubject.asObservable().pipe(
      filter((notification): notification is OrderModel => notification !== null)
    );
  }

  public onNotificationRead(): Observable<string> {
    return this.notificationReadSubject.asObservable().pipe(
      filter((notificationId): notificationId is string => notificationId !== null)
    );
  }

  public onNotificationReadAll(): Observable<boolean> {
    return this.notificationReadAllSubject.asObservable();
  }

  public async joinGroup(userName: string): Promise<void> {
    try {
      await this.hubConnection.invoke('JoinGroup', userName);
    } catch (error) {
      console.error('Error joining group:', error);
    }
  }

  public async leaveGroup(userName: string): Promise<void> {
    try {
      await this.hubConnection.invoke('LeaveGroup', userName);
    } catch (error) {
      console.error('Error leaving group:', error);
    }
  }

  // API Methods
  public getUserNotifications(params: any): Observable<any> {
    return this.commonService.get('Notification/GetUserNotifications', params);
  }

  public markAsRead(notificationId: any): Observable<any> {
    return this.commonService.put(`Notification/MarkAsRead/${notificationId}`, {});
  }

  public markAllAsRead(userName: any): Observable<any> {
    return this.commonService.put('Notification/MarkAllAsRead', { userName });
  }

  public getUnreadCount(userName: any): Observable<any> {
    return this.commonService.get('Notification/GetUnreadCount', { userName });
  }

  public deleteNotification(notificationId: any): Observable<any> {
    return this.commonService.delete(`Notification/Delete/${notificationId}`);
  }

  public getCurrentNotifications(): Observable<{ data: OrderModel[], hasMore: boolean }> {
    return this.notificationsSubject.asObservable();
  }

  public getLoadingState(): Observable<boolean> {
    return this.loadingSubject.asObservable();
  }

  public loadNotifications(params: OrderModel): void {
    this.loadingSubject.next(true);

    this.getUserNotifications(params).subscribe({
      next: (response) => {
        const currentNotifications = this.notificationsSubject.value.data;
        let newNotifications: OrderModel[];

        if (params.currentPage === 1) {
          newNotifications = response.data;
        } else {
          newNotifications = [...currentNotifications, ...response.data];
        }

        this.notificationsSubject.next({
          data: newNotifications,
          hasMore: response.data.length === params.pageSize
        });
        this.loadingSubject.next(false);
      },
      error: (error) => {
        console.error('Error loading notifications:', error);
        this.loadingSubject.next(false);
      }
    });
  }

  public getUnreadCountState(): Observable<number> {
    return this.unreadCountSubject.asObservable();
  }

  public updateUnreadCount(userName: string): void {
    this.getUnreadCount(userName).subscribe({
      next: (count) => {
        this.unreadCountSubject.next(count);
      },
      error: (error) => {
        console.error('Error getting unread count:', error);
      }
    });
  }

  public refreshNotifications(userName: string) {
    this.updateUnreadCount(userName);

    const filter = new OrderModel();
    filter.currentPage = 1;
    filter.pageSize = 10;
    this.loadNotifications(filter);
  }
}