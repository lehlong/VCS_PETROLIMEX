import { Component, OnInit } from '@angular/core';
import { ShareModule } from '../../shared/share-module';
import { BaseFilter } from '../../models/base.model';
import { OrderService } from '../../services/business/order.service';

@Component({
  selector: 'app-get-goods-display',
  standalone: true,
  imports: [ShareModule],
  templateUrl: './get-goods-display.component.html',
  styleUrl: './get-goods-display.component.scss'
})
export class GetGoodsDisplayComponent implements OnInit {
  isFullscreen: boolean = false;
  companyCode?: string = localStorage.getItem('companyCode')?.toString()
  warehouseCode?: string = localStorage.getItem('warehouseCode')?.toString()
  filter: BaseFilter = {
    orgCode: localStorage.getItem('companyCode')?.toString(),
    warehouseCode: localStorage.getItem('warehouseCode')?.toString(),
    currentPage: 0,
    pageSize: 0,
    keyWord: ''
  }
  lstArrangePumpNozzle: any[] = [];
  constructor(private _service: OrderService,) { }
  ngOnInit() {
    this.toggleFullscreen(true);
    this.ArrangePumpNozzle();
    setInterval(() => { this.ArrangePumpNozzle(); }, 5000);
  }
  title: string = '';
  speechNotify(vehicleCode: string, pump : string): void {
    const space = vehicleCode.split('').join(' ');
    const utterance = new SpeechSynthesisUtterance(
      `Xin mời xe có biển số, ${space}, vào ${pump}.`
    );
    utterance.lang = 'vi-VN';
    utterance.rate = 0.65;
    window.speechSynthesis.speak(utterance);
  }
  ArrangePumpNozzle() {
    this._service.ArrangePumpNozzle(this.filter).subscribe({
      next: (data) => {
        data.forEach((item: any) => {
          if (item.order.length > 4) {
            item.order = item.order.slice(0, 4);
          }
          if (item.order.length < 4) {
            for (let i = item.order.length; i < 4; i++) {
              item.order.push('');
            }
          }
        });
        if (this.lstArrangePumpNozzle.length == data.length) {

          var changes = this.compareAndAddChanges(data, this.lstArrangePumpNozzle);
          changes.forEach((item: any, index) => {
            if (item.changes && item.order[0] != '') {
              setTimeout(() => {
                console.log(`Xin mời xe có biển số ${item.order[0]}, vào ${item.pumpThroatName}.`)
                this.title = `Xin mời xe có biển số ${item.order[0]}, vào ${item.pumpThroatName}.`
                this.speechNotify(item.order[0], item.pumpThroatName);
              }, index * 500);
            }
          });
        }
        this.lstArrangePumpNozzle = data;
        console.log('OK!')
      },
      error: (err) => {
        console.error('Error fetching orders:', err);
      }
    });
  }

  toggleFullscreen(check: boolean) {
    this.isFullscreen = check;
    if (check == true) {
      document.documentElement.requestFullscreen()
    } else {
      document.exitFullscreen()
        .then(() => {
        })
        .catch(() => {
        })
    }
  }
  compareAndAddChanges(arr1: any[], arr2: any[]): any[] {
    const result = [];
    if (arr1.length !== arr2.length) {
      throw new Error('Arrays must be of the same length to compare');
    }
    for (let i = 0; i < arr1.length; i++) {
      const obj1 = arr1[i];
      const obj2 = arr2[i];

      const changes = this.compareOrder(obj1.order, obj2.order);
      const updatedObj = { ...obj1, changes };
      result.push(updatedObj);
    }
    return result;
  }
  compareOrder(order1: string[], order2: string[]): boolean {
    return JSON.stringify(order1) !== JSON.stringify(order2);
  }
}