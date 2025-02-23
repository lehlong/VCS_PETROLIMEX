import { Component } from '@angular/core';
import { ShareModule } from '../../shared/share-module';
import { ConfigDisplayService } from '../../services/system-manager/config-display.service';
import { BaseFilter } from '../../models/base.model';
import { GlobalService } from '../../services/global.service';
import { NzMessageService } from 'ng-zorro-antd/message';
import { Router } from '@angular/router';

@Component({
  selector: 'app-order-display-list',
  standalone: true,
  imports: [ShareModule],
  templateUrl: './order-display-list.component.html',
  styleUrl: './order-display-list.component.scss'
})
export class OrderDisplayListComponent {
orgCode?: string = localStorage.getItem('companyCode')?.toString()
  warehouseCode?: string = localStorage.getItem('warehouseCode')?.toString()

  itemEdit: any = {
    id: '',
    name: '',
    orgCode: '',
    warehouseCode: '',
    cfrom:'',
    cto:'',
    display:'',
    isActive: true
  }

  lstConfigDisplay : any[] = []
  visible: boolean = false
  edit: boolean = false
  
  filter = new BaseFilter()
  
  loading: boolean = false
  constructor(
    private _s : ConfigDisplayService,
    private globalService: GlobalService,
    private message: NzMessageService,
  private router : Router
  ) {
    this.globalService.setBreadcrumb([
      {
        name: 'Cấu hình màn hình',
        path: 'system-manager/config-display',
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
  openDisplay(data: any) {
    if(data.display == '01'){
      this.router.navigate([`/business/order-display/${data.id}`])
    }else{
      this.router.navigate([`/business/get-goods-display/${data.id}`])
    }
    
  }

  search() {
    if (this.globalService.isValidSelected()) {
      this.message.create('warning', `Vui lòng chọn đơn vị và kho`);
    } else {
      this.filter.orgCode = this.orgCode
      this.filter.warehouseCode = this.warehouseCode
      this._s.searchConfigDisplay(this.filter).subscribe({
        next: (data) => {
          this.lstConfigDisplay = data.data
        },
        error: (response) => {
          console.log(response)
        },
      })
    }
  }
  
  submitForm() {
    this.itemEdit.orgCode = this.orgCode
    this.itemEdit.warehouseCode = this.warehouseCode
    if (this.edit) {
      this._s.updateConfigDisplay(this.itemEdit).subscribe({
        next: (data) => {
          this.search();
        }
      })
    } else {
      this._s.createConfigDisplay(this.itemEdit).subscribe({
        next: (data) => {
          this.search();
        }
      })
    }
  }

  close() {
    this.visible = false
    this.reset();
  }

  reset() {
    this.itemEdit = {
      id: '',
      name: '',
      orgCode: '',
      warehouseCode: '',
      cfrom:'',
      cto:'',
      display:'',
      isActive: true
    }
  }

  openCreate() {
    if (this.globalService.isValidSelected()) {
      this.message.create('warning', `Vui lòng chọn đơn vị và kho`);
    } else {
      this.edit = false
      this.visible = true
    }

  }
  openEdit(item : any) {
    this.itemEdit = item
    this.edit = true
    this.visible = true
  }
}
