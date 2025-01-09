import {Injectable} from '@angular/core';
import {Observable} from 'rxjs';
import {CommonService} from '../common.service';

@Injectable({
  providedIn: 'root',
})
export class PositionService {
  constructor(private commonService: CommonService) {}

  searchPosition(params: any): Observable<any> {
    return this.commonService.get('Position/Search', params);
  }

  getall(): Observable<any> {
    return this.commonService.get('Position/GetAll');
  }

  createPosition(params: any): Observable<any> {
    return this.commonService.post('Position/Insert', params);
  }

  updatePosition(params: any): Observable<any> {
    return this.commonService.put('Position/Update', params);
  }

  exportExcelPosition(params: any): Observable<any> {
    return this.commonService.downloadFile('Position/Export', params);
  }

  deletePosition(id: string | number): Observable<any> {
    return this.commonService.delete(`Position/Delete/${id}`);
  }
}
