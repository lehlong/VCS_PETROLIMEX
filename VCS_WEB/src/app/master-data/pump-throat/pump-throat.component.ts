import { Component } from '@angular/core'
import { ShareModule } from '../../shared/share-module'
import { GlobalService } from '../../services/global.service'
import { BaseFilter } from '../../models/base.model'
import { NzMessageService } from 'ng-zorro-antd/message'
import { PumpThroatService } from '../../services/master-data/pump-throat.service'
import { PumpRigService } from '../../services/master-data/pump-rig.service'
import { GoodsService } from '../../services/master-data/goods.service'
import { MASTER_DATA_RIGHTS } from '../../shared/constants'
@Component({
  selector: 'app-pump-rig',
  standalone: true,
  imports: [ShareModule],
  templateUrl: './pump-throat.component.html',
  styleUrl: './pump-throat.component.scss',
})
export class PumpThroatComponent {
  orgCode?: string = localStorage.getItem('companyCode')?.toString()
  warehouseCode?: string = localStorage.getItem('warehouseCode')?.toString()

  itemEdit: any = {
    code: '',
    name: '',
    orgCode: '',
    warehouseCode: '',
    pumpRigCode: '',
    goodsCode: '',
    capacity: '',
    tdhCode: '',
    tdhE5Code: '',
    isActive: true
  }

  lstPumpThroat : any[] = []
  lstGoods : any[] = []
  lstPumpRig : any[] = []


  visible: boolean = false
  edit: boolean = false
  
  filter = new BaseFilter()
  
  loading: boolean = false
  MASTER_DATA_RIGHTS = MASTER_DATA_RIGHTS
  constructor(
    private _s : PumpThroatService,
    private _sRig : PumpRigService,
    private _sGoods : GoodsService,
    private globalService: GlobalService,
    private message: NzMessageService,
  ) {
    this.globalService.setBreadcrumb([
      {
        name: 'Danh sách họng bơm',
        path: 'master-data/pump-throat',
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
    this.getPumpRig()
    this.getGoods()
    this.search()
  }

  search() {
    if (this.globalService.isValidSelected()) {
      this.message.create('warning', `Vui lòng chọn đơn vị và kho`);
    } else {
      this.filter.orgCode = this.orgCode
      this.filter.warehouseCode = this.warehouseCode
      this._s.searchPumpThroat(this.filter).subscribe({
        next: (data) => {
          this.lstPumpThroat = data.data
        },
        error: (response) => {
          console.log(response)
        },
      })
    }
  }

  getPumpRig(){
    if (this.globalService.isValidSelected()) {
      this.message.create('warning', `Vui lòng chọn đơn vị và kho`);
    } else {
      this.filter.orgCode = this.orgCode
      this.filter.warehouseCode = this.warehouseCode
      this._sRig.searchPumpRig(this.filter).subscribe({
        next: (data) => {
          this.lstPumpRig = data.data
        },
        error: (response) => {
          console.log(response)
        },
      })
    }
  }

  getGoods(){
    this._sGoods.getall().subscribe({
      next: (data) => {
        this.lstGoods = data
      },
      error: (response) => {
        console.log(response)
      },
    })
  }
  
  submitForm() {
    this.itemEdit.orgCode = this.orgCode
    this.itemEdit.warehouseCode = this.warehouseCode
    if (this.edit) {
      this._s.updatePumpThroat(this.itemEdit).subscribe({
        next: (data) => {
          this.search();
        }
      })
    } else {
      this._s.createPumpThroat(this.itemEdit).subscribe({
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
      pumpRigCode: '',
      goodsCode: '',
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

  getNamePumpRig(code: any){
    return `[${code}] - ` + this.lstPumpRig.find(x => x.code == code)?.name
  }
  getNameGoods(code: any){
    return `[${code}] - ` + this.lstGoods.find(x => x.code == code)?.name
  }
}
