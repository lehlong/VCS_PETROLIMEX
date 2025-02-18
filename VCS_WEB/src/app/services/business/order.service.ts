import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { BehaviorSubject, Observable } from 'rxjs';
import { OrderModel } from '../../models/bussiness/order.model';
import { CommonService } from '../common.service';
import * as signalR from '@microsoft/signalr';
import { filter } from 'rxjs/operators';
import { BaseFilter } from '../../models/base.model';
import { tap } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class OrderService {
  private hubUrl = `${environment.baseApiUrl.replace('/api', '')}/order`;
  private hubConnection!: signalR.HubConnection;
  private orderListSubject = new BehaviorSubject<OrderModel[]>([]);

  constructor(private commonService: CommonService) { }

  // // API Methods
  // GetOrder(filter: BaseFilter): Observable<any> {
  //   return this.commonService.get('Order/GetOrder', filter).pipe(
  //     tap((data: any) => {
  //       this.orderListSubject.next(data);
  //     })
  //   );
  // }

  GetList(filter: BaseFilter): Observable<any> {
    return this.commonService.getWithoutLoading(`Order/GetOrder`, filter);
  }
  GetListWithoutLoading(filter: BaseFilter): Observable<any> {
    return this.commonService.getWithoutLoading(`Order/GetOrderDisplay`, filter);
  }

  ArrangePumpNozzle(filter: BaseFilter): Observable<any> {
    return this.commonService.getWithoutLoading(`Order/ArrangePumpNozzle`, filter);
  }

  Add(order: OrderModel): Observable<any> {
    return this.commonService.post('Order/Add', order);
  }

  UpdateOrderCall(params: any): Observable<any> {
    return this.commonService.put('Order/UpdateOrderCall', params);
  }

  UpdateStatus(params: any): Observable<any> {
    return this.commonService.putWithoutLoading('Order/UpdateStatus', params);
  }

  UpdateOrderCome(params: any): Observable<any> {
    return this.commonService.put('Order/UpdateOrderCome', params);
  }

  CheckTicket(headerId: string): Observable<any> {
    return this.commonService.getWithoutLoading(`Order/CheckTicket?headerId=${headerId}`);
  }
  GetTicket(headerId: string): Observable<any> {
    return this.commonService.get(`Order/GetTicket?headerId=${headerId}`);
  }

  // SignalR Methods
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

      await this.hubConnection.start();
      this.registerSignalREvents();
      console.log('SignalR Connection Initialized');
    } catch (error) {
      console.error('SignalR Connection Error:', error);
    }
  }

  private registerSignalREvents(): void {
    this.hubConnection.on('ORDER_LIST_CHANGED', (orders: OrderModel[]) => {
      this.orderListSubject.next(orders);
    });

    this.hubConnection.on('ORDER_CALL', (orders: OrderModel[]) => {
      this.orderListSubject.next(orders);
    });

    this.hubConnection.on('ORDER_COME', (orders: OrderModel[]) => {
      this.orderListSubject.next(orders);
    });
  }
  public isConnected(): boolean {
    return this.hubConnection?.state === signalR.HubConnectionState.Connected;
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
      if (this.hubConnection?.state === signalR.HubConnectionState.Connected) {
        await this.hubConnection.invoke('LeaveGroup', userName);
      }
    } catch (error) {
      console.error('Error leaving group:', error);
    }
  }

  // Observable để component có thể subscribe
  public getOrderList(): Observable<OrderModel[]> {
    return this.orderListSubject.asObservable();
  }

  // Cleanup method
  public async disconnect() {
    try {
      if (this.hubConnection?.state === signalR.HubConnectionState.Connected) {
        await this.hubConnection.stop();
      }
    } catch (error) {
      console.error('Error disconnecting:', error);
    }
  }
}