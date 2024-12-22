import { Component } from '@angular/core'
import { ShareModule } from '../../shared/share-module'
import { GlobalService } from '../../services/global.service'
import { BaseFilter } from '../../models/base.model'
import { NzMessageService } from 'ng-zorro-antd/message'
import { CameraService } from '../../services/master-data/camera.service'
@Component({
  selector: 'app-camera',
  standalone: true,
  imports: [ShareModule],
  templateUrl: './camera.component.html',
  styleUrl: './camera.component.scss',
})
export class CameraComponent {
  orgCode?: string = localStorage.getItem('companyCode')?.toString()
  warehouseCode?: string = localStorage.getItem('warehouseCode')?.toString()

  itemEdit: any = {
    code: '',
    name: '',
    orgCode: '',
    warehouseCode: '',
    ip:'',
    username:'',
    password:'',
    rtsp:'',
    stream:'',
    isIn : false,
    isOut: false,
    isRecognition: false,
    isActive: true
  }

  lstCamera : any[] = []
  visible: boolean = false
  edit: boolean = false
  
  filter = new BaseFilter()
  
  loading: boolean = false
  constructor(
    private _s : CameraService,
    private globalService: GlobalService,
    private message: NzMessageService,
  ) {
    this.globalService.setBreadcrumb([
      {
        name: 'Danh sách camera',
        path: 'master-data/camera',
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
      this._s.searchCamera(this.filter).subscribe({
        next: (data) => {
          this.lstCamera = data.data
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
      this._s.updateCamera(this.itemEdit).subscribe({
        next: (data) => {
          this.search();
        }
      })
    } else {
      this._s.createCamera(this.itemEdit).subscribe({
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
      ip: '',
      username: '',
      password: '',
      rtsp: '',
      stream: '',
      isIn: false,
      isOut: false,
      isRecognition: false,
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
