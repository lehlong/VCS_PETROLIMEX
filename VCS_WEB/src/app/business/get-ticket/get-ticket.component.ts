import { Component, OnInit, OnDestroy, ViewChild, ElementRef } from '@angular/core'
import { ShareModule } from '../../shared/share-module'
import { BaseFilter } from '../../models/base.model'
import { OrderService } from '../../services/business/order.service'
import { GlobalService } from '../../services/global.service'
import { Subscription } from 'rxjs'
declare var $: any

@Component({
  selector: 'app-get-ticket',
  standalone: true,
  imports: [ShareModule],
  templateUrl: './get-ticket.component.html',
  styleUrl: './get-ticket.component.scss',
})
export class GetTicketComponent implements OnInit, OnDestroy {
  private orderSubscription?: Subscription;
  private readonly SHOULD_PLAY_VOICE = false;
  companyCode?: string = localStorage.getItem('companyCode')?.toString()
  warehouseCode?: string = localStorage.getItem('warehouseCode')?.toString()
  filter: BaseFilter = {
    orgCode: localStorage.getItem('companyCode')?.toString(),
    warehouseCode: localStorage.getItem('warehouseCode')?.toString(),
    currentPage: 0,
    pageSize: 0,
    keyWord: ''
  }
  loading: boolean = false
  lstOrder: any[] = []
  userName: any

  @ViewChild('printSection') printSection!: ElementRef;
  constructor(
    private _service: OrderService,
    private globalService: GlobalService,
  ) {
    this.globalService.setBreadcrumb([
      {
        name: 'Thứ tự lấy ticket',
        path: 'business/get-ticket',
      },
    ])
    this.globalService.getLoading().subscribe((value) => {
      this.loading = value
    })
    const UserInfo = this.globalService.getUserInfo()
    this.userName = UserInfo?.userName
  }

  ngOnInit() {
    this.getOrder();
    this.setupSignalRConnection();
  }

  async setupSignalRConnection() {
    if (!this._service.isConnected()) {
      await this._service.initializeConnection();
      await this._service.joinGroup(this.companyCode || '');
    }
    this.orderSubscription = this._service.getOrderList().subscribe(orders => {
      if (orders) {
        console.log('Nhận được đơn hàng mới:', orders);
        this.lstOrder = orders;
      }
    });
  }


  async ngOnDestroy() {
    if (this.orderSubscription) {
      this.orderSubscription.unsubscribe();
    }
    this.globalService.setBreadcrumb([]);
  }

  getOrder() {
    this._service.GetOrder(this.filter).subscribe({
      next: (data) => {
        this.lstOrder = data;
      },
      error: (err) => {
        console.error('Error fetching orders:', err);
      }
    });
  }

  checkTicket(headerId: string) {
    this._service.CheckTicket(headerId).subscribe({
      next: (data) => {
        console.log(data)
      },
      error: (err) => {
        console.error('Error fetching orders:', err);
      }
    });
  }

  updateOrderCall(params: any) {
    console.log("params", params);

    this._service.UpdateOrderCall(params).subscribe({
      next: (response) => {
      },
      error: (err) => {
        console.error('Error calling order:', err);
      }
    });
  }

  updateOrderCome(params: any) {
    const updatedParams = {
      ...params,
      isCome: true,
      isDone: false
    };

    this._service.UpdateOrderCome(updatedParams).subscribe({
      next: (response) => {
      },
      error: (err) => {
        console.error('Error marking order as come:', err);
      }
    });
  }
  updateOrderDone(params: any) {
    const updatedParams = {
      ...params,
      isDone: true
    };

    this._service.UpdateOrderCome(updatedParams).subscribe({
      next: (response) => {
      },
      error: (err) => {
        console.error('Error marking order as come:', err);
      }
    });
  }

  printTicket() {
    let printContent = this.printSection.nativeElement.innerHTML;
    let originalContent = document.body.innerHTML;

    document.body.innerHTML = printContent;
    window.print();
    document.body.innerHTML = originalContent;
    window.location.reload(); // To restore events like click handlers
  }
}
