import { Injectable } from '@angular/core'
import { Observable } from 'rxjs'
import { CommonService } from '../common.service'

@Injectable({
  providedIn: 'root',
})
export class PumpThroatService {
  constructor(private commonService: CommonService) {}

  searchPumpThroat(params: any): Observable<any> {
    return this.commonService.get('PumpThroat/Search', params)
  }

  getAll(): Observable<any> {
    return this.commonService.get('PumpThroat/GetAll')
  }

  createPumpThroat(params: any): Observable<any> {
    return this.commonService.post('PumpThroat/Insert', params)
  }

  updatePumpThroat(params: any): Observable<any> {
    return this.commonService.put('PumpThroat/Update', params)
  }

  exportExcelPumpThroat(params: any): Observable<any> {
    return this.commonService.downloadFile('PumpThroat/Export', params)
  }

  deletePumpThroat(id: string | number): Observable<any> {
    return this.commonService.delete(`PumpThroat/Delete/${id}`)
  }
}
