import { Component, OnInit, OnDestroy, ViewChild, ElementRef } from '@angular/core'
import { ShareModule } from '../../shared/share-module'
import { BaseFilter } from '../../models/base.model'
import { OrderService } from '../../services/business/order.service'
import { GlobalService } from '../../services/global.service'
import { Subscription } from 'rxjs'
import { NzMessageService } from 'ng-zorro-antd/message'
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
  ticketDetail: any = {
    companyName: '',
    ticketNumber: '',
    vehicle: '',
    driverName: '',
    custmerName: '',
    chuyenVt: '',
    detail: []
  }

  @ViewChild('printSection') printSection!: ElementRef;
  constructor(
    private _service: OrderService,
    private globalService: GlobalService,
    private message: NzMessageService,
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
    this.getList();
    setInterval(() => { this.getList(); }, 5000)
    //this.setupSignalRConnection();
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


  ngOnDestroy() {
    this.globalService.setBreadcrumb([])
  }



  getList() {
    this._service.GetList(this.filter).subscribe({
      next: (data) => {
        this.lstOrder = data;
      },
      error: (err) => {
        console.error('Error fetching orders:', err);
      }
    });
  }


  updateStatus(header: any, status: string) {
    header.statusProcess = status;
    this._service.UpdateStatus(header).subscribe({
      next: (data) => {
        this.getList();
      }
    });
  }

  updateOrder(header: any) {
    header.statusVehicle = "03";
    this._service.CheckTicket(header.id).subscribe({
      next: (data) => {
        if (data) {
          this._service.UpdateOrder(header.id).subscribe({
            next: (data) => {
              this.getList();
            }
          });

        } else {
          this.message.create('error', `Phương tiện chưa có tiket`);
          this.getList();
        }
      },
      error: (err) => {
        console.error('Error fetching orders:', err);
      }
    });
  }

  checkTicket(headerId: string) {
    this._service.CheckTicket(headerId).subscribe({
      next: (data) => {
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

  UpdateNote(i: any) {
    console.log(i)
  }

  printTicket(headerId: string) {

    this._service.GetTicket(headerId).subscribe({
      next: (data) => {
        this.ticketDetail = data;
        var sum = 0;
        for (var i = 0; i < data.detail.length; i++) {
          sum += data.detail[i].tongDuXuat
        }
        this.ticketDetail.sum = sum;

        setTimeout(() => {
          let printContent = this.printSection.nativeElement.innerHTML;
          let originalContent = document.body.innerHTML;

          document.body.innerHTML = printContent;
          window.print();
          document.body.innerHTML = originalContent;
          window.location.reload();
        }, 200)

      },
      error: (err) => {
        console.error('Error fetching orders:', err);
      }
    });

  }
}
