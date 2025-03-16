import { Component, OnInit } from '@angular/core';
import { ShareModule } from '../../../shared/share-module';
import { GlobalService } from '../../../services/global.service';
import { ActivatedRoute } from '@angular/router';
import { HeaderService } from '../../../services/business/header.service';
import { environment } from '../../../../environments/environment';

@Component({
  selector: 'app-history-detail',
  standalone: true,
  imports: [ShareModule],
  templateUrl: './history-detail.component.html',
  styleUrl: './history-detail.component.scss'
})
export class HistoryDetailComponent implements OnInit {

  wareHouseCode?:string =localStorage.getItem('warehouseCode')?.toString()

  loading: boolean = false
  isSubmit: boolean = false
  lstHistory: any = [];
  dataIn: any = [];
  dataOut: any = [];
  imagesIn: any = [];
  imagesOut: any = [];
  imageErrors: boolean[] = [];
  imageErrorsOut: boolean[] = [];
  ImageApiUrl = environment.imageApiUrl;
  constructor(
    private _service: HeaderService,
    private globalService: GlobalService,
    private route: ActivatedRoute
  ) {
    this.globalService.setBreadcrumb([
      {
        name: 'Chi tiết lịch sử',
        path: 'business/history-detail',
      },
    ])
    this.globalService.getLoading().subscribe((value) => {
      this.loading = value
    })
  }

  
  ngOnInit() {
    this.search();
    console.log(this.imagesIn)
  }
  
onImageError(index: number, type: 'in' | 'out') {
  if (type === 'in') {
    this.imageErrors[index] = true;
  } else {
    this.imageErrorsOut[index] = true;
  }
}
  search() {
    this.isSubmit = false
    this.route.paramMap.subscribe((params) => {
      const id = params.get('id');
      if (id) {
        this._service.GetHistoryDetail(id).subscribe({
          next: (data) => {
            this.lstHistory = data
            this.dataIn = data.detailDOs
            this.dataOut = data.detailTgbx
            this.imagesIn = data.imagesIn
            this.imagesOut = data.imagesOut
          },
          error: (response) => {
            console.log(response)
          },
        })
      }
    });
    
  }
}
