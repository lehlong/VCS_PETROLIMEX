import { Injectable } from '@angular/core'
import { Observable } from 'rxjs'
import { CommonService } from '../common.service'

@Injectable({
  providedIn: 'root',
})
export class ReportService {
  constructor(private commonService: CommonService) { }

  getBaoCaoChiTietXe(params: any): Observable<any> {
    return this.commonService.get('Order/BaoCaoChiTietXe', params)
  }
  downloadFile(params: any): Observable<any> {
    return this.commonService.downloadFile('Order/Download', params);
  }
}
