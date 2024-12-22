import { Component, OnInit, ViewChild } from '@angular/core'
import { ShareModule } from '../../shared/share-module'
import { GlobalService } from '../../services/global.service'
import { FormGroup, Validators, NonNullableFormBuilder } from '@angular/forms'
import { NzMessageService } from 'ng-zorro-antd/message'
import { NzTreeComponent, NzFormatEmitEvent, NzTreeNodeOptions } from 'ng-zorro-antd/tree'
import { OrganizeService } from '../../services/system-manager/organize.service'
import { WarehouseService } from '../../services/master-data/warehouse.service'
@Component({
  selector: 'app-warehouse',
  standalone: true,
  imports: [ShareModule],
  templateUrl: './warehouse.component.html',
  styleUrl: './warehouse.component.scss',
})
export class WarehouseComponent implements OnInit {
  @ViewChild('treeCom', { static: false }) treeCom!: NzTreeComponent
  searchValue = ''
  nodes: any = []
  originalNodes: any[] = []
  visible: boolean = false
  edit: boolean = false
  companyCode?: string = localStorage.getItem('companyCode')?.toString()
  titleParent: string = ''
  itemEdit: any = {
    code: '',
    name: '',
    orgCode: '',
    isActive: true
  }
  lstWarehouse: any[] = [];
  constructor(
    private _service: OrganizeService,
    private _w : WarehouseService,
    private fb: NonNullableFormBuilder,
    private globalService: GlobalService,
    private message: NzMessageService,
  ) {
    this.globalService.setBreadcrumb([
      {
        name: 'Danh sách kho',
        path: 'master-data/warehouse',
      },
    ])
  }

  ngOnInit(): void {
    console.log(this.companyCode)
    this.getLstWarehouse()
  }

  getLstWarehouse(){
    this._w.getByOrg(this.companyCode).subscribe({
      next: (res) => {
        this.lstWarehouse = res;
      }
    })
  }

  close() {
    this.visible = false
    this.reset()
  }

  reset() {
    this.itemEdit = {
      code: '',
      name: '',
      orgCode: '',
      isActive: true
    }
  }
  openCreate() {
    if(!this.companyCode){
      this.message.create('warning', `Vui lòng chọn đơn vị`);
    }else{
      this.visible = true;
      this.edit = false;
    }
  }
  openEdit(item : any) {
    this.itemEdit = item
    this.visible = true
    this.edit = true
  }
 
  submitForm() {
    this.itemEdit.orgCode = this.companyCode
    if (this.edit) {
      this._w.updateWarehouse(this.itemEdit).subscribe({
        next: (data) => {
          this.getLstWarehouse();
        }
      })
    } else {
      this._w.createWarehouse(this.itemEdit).subscribe({
        next: (data) => {
          this.getLstWarehouse();
        }
      })
    }
  }
}