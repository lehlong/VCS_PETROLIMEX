import { Injectable } from '@angular/core'
import { Observable } from 'rxjs'
import { CommonService } from '../common.service'

@Injectable({
  providedIn: 'root',
})
export class HeaderService {
  constructor(private commonService: CommonService) {}

  searchHeader(params: any): Observable<any> {
    return this.commonService.get('Header/Search', params)
  }

  GetHistoryDetail(headerId: string): Observable<any> {
    return this.commonService.get(`Header/GetHistoryDetail?headerId=${headerId}`)
  }
}
