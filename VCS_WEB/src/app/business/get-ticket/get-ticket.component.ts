import { Component, OnInit, OnDestroy } from '@angular/core'
import { ShareModule } from '../../shared/share-module'
import { BaseFilter } from '../../models/base.model'
import { OrderService } from '../../services/business/order.service'
import { GlobalService } from '../../services/global.service'
import { Subscription } from 'rxjs'

@Component({
  selector: 'app-get-ticket',
  standalone: true,
  imports: [ShareModule],
  templateUrl: './get-ticket.component.html',
  styleUrl: './get-ticket.component.scss',
})
export class GetTicketComponent implements OnInit, OnDestroy {
  private orderSubscription?: Subscription;
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
  }

  async ngOnInit() {
    await this._service.initializeConnection();
    await this._service.joinGroup(this.companyCode || '');
    this.getOrder();

    // Subscribe to real-time updates
    this.orderSubscription = this._service.getOrderList().subscribe(orders => {
      if (orders) {
        this.lstOrder = orders;
        console.log("listOrder", this.lstOrder);

      }
    });
  }

  ngOnDestroy() {
    this.globalService.setBreadcrumb([]);
    if (this.orderSubscription) {
      this.orderSubscription.unsubscribe();
    }
    this._service.leaveGroup(this.companyCode || '');
    this._service.disconnect();
  }

  getOrder() {
    this._service.GetOrder(this.filter).subscribe({
      next: (data) => {
        this.lstOrder = data.data
        console.log("this.filter", this.filter);

        console.log("data", this.lstOrder);

      },
      error: (err) => {
        console.error('Error fetching orders:', err);
      }
    })
  }

  updateOrderCall(params: any) {
    console.log("params", params);

    this._service.UpdateOrderCall(params).subscribe({
      next: (response) => {
        this.getOrder()
      },
      error: (err) => {
        console.error('Error calling order:', err);

      }
    });
  }

  updateOrderCome(params: any) {

    this._service.UpdateOrderCome(params).subscribe({
      next: (response) => {
        this.getOrder()
      },
      error: (err) => {
        console.error('Error marking order as come:', err);

      }
    });
  }
}
