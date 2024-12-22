import { Injectable } from '@angular/core'
import { Observable } from 'rxjs'
import { CommonService } from '../common.service'

@Injectable({
  providedIn: 'root',
})
export class PumpRigService {
  constructor(private commonService: CommonService) {}

  searchPumpRig(params: any): Observable<any> {
    return this.commonService.get('PumpRig/Search', params)
  }

  getAll(): Observable<any> {
    return this.commonService.get('PumpRig/GetAll')
  }

  createPumpRig(params: any): Observable<any> {
    return this.commonService.post('PumpRig/Insert', params)
  }

  updatePumpRig(params: any): Observable<any> {
    return this.commonService.put('PumpRig/Update', params)
  }

  exportExcelPumpRig(params: any): Observable<any> {
    return this.commonService.downloadFile('PumpRig/Export', params)
  }

  deletePumpRig(id: string | number): Observable<any> {
    return this.commonService.delete(`PumpRig/Delete/${id}`)
  }
}
