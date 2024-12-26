import { Component, OnInit } from '@angular/core'
import { ShareModule } from '../shared/share-module';
import { BaseFilter } from '../models/base.model';
import { CameraService } from '../services/master-data/camera.service';
import { GlobalService } from '../services/global.service';
import { NzMessageService } from 'ng-zorro-antd/message'
declare var google: any
declare var flvjs: any
@Component({
  selector: 'app-home',
  standalone: true,
  imports: [ShareModule],
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss',
})
export class HomeComponent implements OnInit {
  orgCode?: string = localStorage.getItem('companyCode')?.toString()
  warehouseCode?: string = localStorage.getItem('warehouseCode')?.toString()
  lstCamera: any[] = []
  filter = new BaseFilter()

  constructor(private _cameraService: CameraService,
    private _globalService: GlobalService,
    private message: NzMessageService,
  ) { }
  ngOnInit(): void {
    this.getCamera();
  }

  getCamera() {
    if (this._globalService.isValidSelected()) {
      this.message.create('warning', `Vui lòng chọn đơn vị và kho`);
    } else {
      this.filter.orgCode = this.orgCode
      this.filter.warehouseCode = this.warehouseCode
      this._cameraService.searchCamera(this.filter).subscribe({
        next: (data) => {
          this.lstCamera = data.data
          if (this.lstCamera.length != 0) {
            this.lstCamera.forEach((c) => {
              var video = document.createElement('video');
              video.id = c.code
              video.muted = true
              video.autoplay = true

              var divIn = document.getElementById('lstCameraIn');
              var divOut = document.getElementById('lstCameraOut');
              if (c.isIn) {
                divIn?.appendChild(video);
              }
              if (c.isOut) {
                divOut?.appendChild(video);
              }

              var config = {
                enableStashBuffer: false,
                autoCleanupSourceBuffer: true,
                stashInitialSize: 128,
                seekType: 'range',
                lazyLoad: true,
                lazyLoadMaxDuration: 3,
                lazyLoadRecoverDuration: 2,
              };
              if (flvjs.isSupported()) {
                var stream = document.getElementById(c.code);
                var flvPlayer = flvjs.createPlayer({
                  type: 'flv',
                  url: c.stream,
                  isLive: true,
                  hasAudio: false,
                }, config);
                flvPlayer.attachMediaElement(stream);
                flvPlayer.load();
                flvPlayer.play();
              }

            })
          }
        },
        error: (response) => {
          console.log(response)
        },
      })
    }
  }
}
