import { Injectable } from '@angular/core'
import { Observable } from 'rxjs'
import { CommonService } from '../common.service'

@Injectable({
  providedIn: 'root',
})
export class CameraService {
  constructor(private commonService: CommonService) {}

  searchCamera(params: any): Observable<any> {
    return this.commonService.get('Camera/Search', params)
  }

  getAll(): Observable<any> {
    return this.commonService.get('Camera/GetAll')
  }

  createCamera(params: any): Observable<any> {
    return this.commonService.post('Camera/Insert', params)
  }

  updateCamera(params: any): Observable<any> {
    return this.commonService.put('Camera/Update', params)
  }

  exportExcelCamera(params: any): Observable<any> {
    return this.commonService.downloadFile('Camera/Export', params)
  }

  deleteCamera(id: string | number): Observable<any> {
    return this.commonService.delete(`Camera/Delete/${id}`)
  }
}
