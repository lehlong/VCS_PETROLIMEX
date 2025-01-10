import { Component, OnInit } from '@angular/core'
import { ShareModule } from '../../shared/share-module'
import { BaseFilter } from '../../models/base.model'
import { OrderService } from '../../services/business/order.service'
import { GlobalService } from '../../services/global.service'
@Component({
  selector: 'app-get-ticket',
  standalone: true,
  imports: [ShareModule],
  templateUrl: './get-ticket.component.html',
  styleUrl: './get-ticket.component.scss',
})
export class GetTicketComponent implements OnInit {
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
  ngOnDestroy() {
    this.globalService.setBreadcrumb([])
  }
  ngOnInit(): void {
    this.getOrder();
  }
  getOrder() {
    this._service.GetOrder(this.filter).subscribe({
      next: (data) => {
        this.lstOrder = data
        console.log(data)
      },
      error: (err) => {

      }
    })
  }
}
