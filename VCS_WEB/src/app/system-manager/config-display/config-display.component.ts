import { Component } from '@angular/core';
import { ShareModule } from '../../shared/share-module';
import { BaseFilter } from '../../models/base.model';
import { ConfigDisplayService } from '../../services/system-manager/config-display.service';
import { GlobalService } from '../../services/global.service';
import { NzMessageService } from 'ng-zorro-antd/message';
import { ADMIN_RIGHTS } from '../../shared/constants';

@Component({
  selector: 'app-config-display',
  standalone: true,
  imports: [ShareModule],
  templateUrl: './config-display.component.html',
  styleUrl: './config-display.component.scss'
})
export class ConfigDisplayComponent {
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
  ADMIN_RIGHTS = ADMIN_RIGHTS
  constructor(
    private _s : ConfigDisplayService,
    private globalService: GlobalService,
    private message: NzMessageService,
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
