import { Component, OnInit, OnDestroy } from '@angular/core';
import { ShareModule } from '../../shared/share-module';
import { OrderService } from '../../services/business/order.service';
import { Subscription } from 'rxjs';
import { OrderModel } from '../../models/bussiness/order.model';
import { GlobalService } from '../../services/global.service';
import { BaseFilter } from '../../models/base.model';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-order-display',
  standalone: true,
  imports: [ShareModule],
  templateUrl: './order-display.component.html',
  styleUrl: './order-display.component.scss'
})
export class OrderDisplayComponent implements OnInit {
  isFullscreen: boolean = false;
  lstOrder: any[] = []
  companyCode?: string = localStorage.getItem('companyCode')?.toString()
  warehouseCode?: string = localStorage.getItem('warehouseCode')?.toString()
  title: string = '';
  count: number = 0;
  vehicleCode: string = '';
  filter: BaseFilter = {
    orgCode: localStorage.getItem('companyCode')?.toString(),
    warehouseCode: localStorage.getItem('warehouseCode')?.toString(),
    currentPage: 0,
    pageSize: 0,
    keyWord: '',
    displayId: '',
  }

  interval: any;


  constructor(private _service: OrderService, private route: ActivatedRoute,) {
  }
  formatNumber(value: number): string {
    return value ? value.toString().padStart(2, '0') : '00';
  }

  ngOnInit() {
    this.toggleFullscreen(true);
    this.getList();
    this.interval = setInterval(() => { this.getList(); }, 5000);
  }
  ngOnDestroy(): void {
    clearInterval(this.interval);
  }

  getList() {
    this.route.paramMap.subscribe({
      next: (params) => {
        var id = params.get('id')
        this.filter.displayId = id ?? undefined;
      },
    })
    this._service.GetListWithoutLoading(this.filter).subscribe({
      next: (data) => {
        this.lstOrder = data;
        var i = this.lstOrder.find(x => x.isVoice === true);
        if (i) {
          this.title = `Xin mời xe có biển số ${i.vehicleCode} vào lấy Ticket`;
          if (i.vehicleCode != this.vehicleCode && this.count < 2) {
            this.vehicleCode = i.vehicleCode;
            this.count++;
            this.speechNotify(i.vehicleCode);
          } else {
            this.count = 0;
          }
        }else{
          this.title = '';
        }
      },
      error: (err) => {
        console.error('Lỗi:', err);
      }
    });
  }

  speechNotify(vehicleCode: string): void {
    const space = vehicleCode.split('').join(' ');
    const utterance = new SpeechSynthesisUtterance(
      `Xin mời xe có biển số, ${space}, vào lấy Tích kê`
    );
    utterance.lang = 'vi-VN';
    utterance.rate = 0.65;
    window.speechSynthesis.speak(utterance);
  }

  toggleFullscreen(check: boolean) {
    this.isFullscreen = check;
    if (check) {
      document.documentElement.requestFullscreen();
    } else {
      document.exitFullscreen()
        .then(() => { })
        .catch(() => { });
    }
  }
}
