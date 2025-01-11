import { Component, OnInit, OnDestroy } from '@angular/core';
import { ShareModule } from '../../shared/share-module';
import { OrderService } from '../../services/business/order.service';
import { Subscription } from 'rxjs';
import { OrderModel } from '../../models/bussiness/order.model';
import { GlobalService } from '../../services/global.service';

@Component({
  selector: 'app-order-display',
  standalone: true,
  imports: [ShareModule],
  templateUrl: './order-display.component.html',
  styleUrl: './order-display.component.scss'
})
export class OrderDisplayComponent implements OnInit, OnDestroy {
  isFullscreen: boolean = false;
  private orderSubscription?: Subscription;
  orders: OrderModel[] = [];
  companyCode?: string = localStorage.getItem('companyCode')?.toString();
  currentCallOrder?: OrderModel;
  userName: any
  loading: boolean = false
  private readonly SHOULD_PLAY_VOICE = true;

  constructor(private orderService: OrderService, private globalService: GlobalService) {
    this.globalService.getLoading().subscribe((value) => {
      this.loading = value;
    });
    const UserInfo = this.globalService.getUserInfo()
    this.userName = UserInfo?.userName
  }

  async ngOnInit() {
    if (!this.orderService.isConnected()) {
      await this.orderService.initializeConnection();
      await this.orderService.joinGroup(this.userName || '');
    }
    this.orderSubscription = this.orderService.getOrderList().subscribe(orders => {
      if (orders) {
        this.orders = orders;
        if (this.SHOULD_PLAY_VOICE) {
          const calledOrder = orders.find(o => o.isCall);
          if (calledOrder && calledOrder !== this.currentCallOrder) {
            this.currentCallOrder = calledOrder;
            this.speechNotify(calledOrder.vehicleCode || '');
          }
        }
      }
    });
  }

  async ngOnDestroy() {
    if (this.orderSubscription) {
      this.orderSubscription.unsubscribe();
    }
    this.globalService.setBreadcrumb([]);
  }

  speechNotify(vehicleCode: string): void {
    const utterance = new SpeechSynthesisUtterance(
      `Xin mời xe có biển số ${vehicleCode} vào lấy Ticket`
    );
    utterance.lang = 'vi-VN';
    window.speechSynthesis.speak(utterance);
  }

  toggleFullscreen(check: boolean) {
    this.isFullscreen = check;
    if (check) {
      document.documentElement.requestFullscreen();
    } else {
      document.exitFullscreen()
        .then(() => { })
        .catch(() => { });
    }
  }
}
