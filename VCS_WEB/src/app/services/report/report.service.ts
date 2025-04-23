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

  baoCaoXeTongHop(params: any): Observable<any> {
    return this.commonService.get('Header/BaoCaoXeTongHop', params)
  }

  baoCaoSanPhamTongHop(params: any): Observable<any> {
    return this.commonService.get('Header/BaoCaoSanPhamTongHop', params)
  }

  ExportExcelBaoCaoXeTongHop(params: any): Observable<any> {
    return this.commonService.downloadFile('Header/ExportExcelBaoCaoXeTongHop', params);
  }
  ExportExcelBaoCaoSanPhamTongHop(params: any): Observable<any> {
    return this.commonService.downloadFile('Header/ExportExcelBaoCaoSanPhamTongHop', params);
  }
}
