import { Component, OnInit } from '@angular/core';
import { ShareModule } from '../../../shared/share-module';
import { GlobalService } from '../../../services/global.service';
import { ActivatedRoute } from '@angular/router';
import { HeaderService } from '../../../services/business/header.service';
import { environment } from '../../../../environments/environment';
import { NzModalModule, NzModalService } from 'ng-zorro-antd/modal';

@Component({
  selector: 'app-history-detail',
  standalone: true,
  imports: [ShareModule],
  templateUrl: './history-detail.component.html',
  styleUrl: './history-detail.component.scss'
})
export class HistoryDetailComponent implements OnInit {

  wareHouseCode?: string = localStorage.getItem('warehouseCode')?.toString()

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
    private route: ActivatedRoute,
    private modalService: NzModalService
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

  }
  openImageModal(imageUrl: string) {
    this.modalService.create({
      nzContent: `<img src="${imageUrl}" style="width: 800px; height: 800px;" />`,
      nzFooter: null, // Không hiển thị footer
      nzMaskClosable: true, // Đóng modal khi click ra ngoài
      nzClosable: false, 
      nzStyle: {
        top: '25vh' ,
      },
      nzBodyStyle: {
        padding: '0',
        background: 'transparent' // Nền trong suốt
      },
      nzWrapClassName: 'image-modal' // Class để tùy chỉnh CSS
    });
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
            this.imageErrors = new Array(this.imagesIn.length).fill(false);
            this.imageErrorsOut = new Array(this.imagesOut.length).fill(false);
          },
          error: (response) => {
            console.log(response)
          },
        })
      }
    });

  }
}
