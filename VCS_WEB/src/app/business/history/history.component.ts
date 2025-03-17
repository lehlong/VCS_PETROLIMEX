import { Component } from '@angular/core';
import { ShareModule } from '../../shared/share-module';
import { AreaFilter } from '../../models/master-data/area.model'
import { GlobalService } from '../../services/global.service'
import { AreaService } from '../../services/master-data/area.service'
import { PaginationResult } from '../../models/base.model'
import { FormGroup, Validators, NonNullableFormBuilder } from '@angular/forms'
import { NzMessageService } from 'ng-zorro-antd/message'
import { OrderService } from '../../services/business/order.service';
import { HeaderService } from '../../services/business/header.service';
import { HeaderFilter } from '../../models/bussiness/header.model';
import { Router } from '@angular/router';
import { NzDatePickerModule } from 'ng-zorro-antd/date-picker';

@Component({
  selector: 'app-history',
  standalone: true,
  imports: [ShareModule, NzDatePickerModule],
  templateUrl: './history.component.html',
  styleUrl: './history.component.scss'
})
export class HistoryComponent {

validateForm: FormGroup = this.fb.group({
    id: ['', [Validators.required]],
    name: ['', [Validators.required]],
    isActive: [true, [Validators.required]],
  })

  isSubmit: boolean = false
  visible: boolean = false
  edit: boolean = false
  filter = new HeaderFilter()
  paginationResult = new PaginationResult()
  loading: boolean = false
  id : string = ''
  constructor(
    private _service: HeaderService,
    private fb: NonNullableFormBuilder,
    private globalService: GlobalService,
    private message: NzMessageService,
    private router: Router,
  ) {
    this.globalService.setBreadcrumb([
      {
        name: 'Lịch sử vào ra',
        path: 'business/history',
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
    this.search()
  }


  search() {
    this.isSubmit = false
    this._service.searchHeader(this.filter).subscribe({
      next: (data) => {
        this.paginationResult = data
      },
      error: (response) => {
        console.error('Lỗi khi lấy dữ liệu:', response, this.filter)
      },
    })
  }

 
  isCodeExist(code: string): boolean {
    return this.paginationResult.data?.some((area: any) => area.code === code)
  }
  
  close() {
    this.visible = false
    this.resetForm()
  }

  reset() {
    this.filter = new HeaderFilter();
    this.filter.vehicleName = '';
    this.filter.vehicleCode = '';
    this.filter.fromDate = undefined;
    this.filter.toDate = undefined;
    this.search();
  }

  openCreate() {
    this.edit = false
    this.visible = true
  }

  resetForm() {
    this.validateForm.reset()
    this.isSubmit = false
  }

  navigate(data: any) {
    this.router.navigate([`/business/history-detail/${data}`])
  }
  // openEdit(data: { code: string; name: string; isActive: boolean }) {
  //   this.validateForm.setValue({
  //     code: data.code,
  //     name: data.name,
  //     isActive: data.isActive,
  //   })
  //   setTimeout(() => {
  //     this.edit = true
  //     this.visible = true
  //   }, 200)
  // }

  pageSizeChange(size: number): void {
    this.filter.currentPage = 1
    this.filter.pageSize = size
    this.search()
  }

  pageIndexChange(index: number): void {
    this.filter.currentPage = index
    this.search()
  }
}
