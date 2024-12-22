import { Component } from '@angular/core'
import { ShareModule } from '../../shared/share-module'
import { GlobalService } from '../../services/global.service'
import { BaseFilter } from '../../models/base.model'
import { NzMessageService } from 'ng-zorro-antd/message'
import { PumpRigService } from '../../services/master-data/pump-rig.service'
@Component({
  selector: 'app-pump-rig',
  standalone: true,
  imports: [ShareModule],
  templateUrl: './pump-rig.component.html',
  styleUrl: './pump-rig.component.scss',
})
export class PumpRigComponent {
  orgCode?: string = localStorage.getItem('companyCode')?.toString()
  warehouseCode?: string = localStorage.getItem('warehouseCode')?.toString()

  itemEdit: any = {
    code: '',
    name: '',
    orgCode: '',
    warehouseCode: '',
    isActive: true
  }

  lstPumpRig : any[] = []
  visible: boolean = false
  edit: boolean = false
  
  filter = new BaseFilter()
  
  loading: boolean = false
  constructor(
    private _s : PumpRigService,
    private globalService: GlobalService,
    private message: NzMessageService,
  ) {
    this.globalService.setBreadcrumb([
      {
        name: 'Danh sách giàn bơm',
        path: 'master-data/pump-rig',
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
    if (this.globalService.isValidSelected()) {
      this.message.create('warning', `Vui lòng chọn đơn vị và kho`);
    } else {
      this.filter.orgCode = this.orgCode
      this.filter.warehouseCode = this.warehouseCode
      this._s.searchPumpRig(this.filter).subscribe({
        next: (data) => {
          this.lstPumpRig = data.data
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
      this._s.updatePumpRig(this.itemEdit).subscribe({
        next: (data) => {
          this.search();
        }
      })
    } else {
      this._s.createPumpRig(this.itemEdit).subscribe({
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
      code: '',
      name: '',
      orgCode: '',
      warehouseCode: '',
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
