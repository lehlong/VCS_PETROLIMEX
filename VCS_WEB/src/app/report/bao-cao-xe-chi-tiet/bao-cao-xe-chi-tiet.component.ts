import { Component } from '@angular/core';
import { HeaderFilter } from '../../models/bussiness/header.model';
import { PaginationResult } from '../../models/base.model';
import { ShareModule } from '../../shared/share-module';
import { ReportService } from '../../services/report/report.service';
import { GlobalService } from '../../services/global.service';
import { WarehouseService } from '../../services/master-data/warehouse.service';

declare const google: any; // Khai báo biến global từ google chart

@Component({
  selector: 'app-bao-cao-xe-chi-tiet',
  standalone: true,
  imports: [ShareModule],
  templateUrl: './bao-cao-xe-chi-tiet.component.html',
  styleUrl: './bao-cao-xe-chi-tiet.component.scss'
})
export class BaoCaoXeChiTietComponent {
 filter = new HeaderFilter();
  paginationResult = new PaginationResult();
  isSubmit: boolean = false;
  loading: boolean = false;
  lstData: any[] = [];
  selectedValue = '';
  date: Date | null = null;
  lstWareHouse: any[] = []
  WareHouse: any
  companyCode?: string = localStorage.getItem('companyCode')?.toString()
  constructor(
    private _service: ReportService,
    private _WarehouseService: WarehouseService,
    private globalService: GlobalService,
  ) {
    this.globalService.setBreadcrumb([
      {
        name: 'Báo cáo chi tiết xe',
        path: 'report/bao-cao-chi-tiet-xe',
      },
    ]);
    this.globalService.getLoading().subscribe((value) => {
      this.loading = value;
    });
  }

  ngOnInit(): void {
    this.search();
    this.getWarehouse();
    this.date = new Date();
  }
  getWarehouse(){
    this._WarehouseService.getByOrg(this.companyCode).subscribe({
      next: (data) => {
        this.lstWareHouse = data;
      },
      error: (response) => {
        console.error(response);
      },
    });
  }
  search() {
    const filterToSend = { ...this.filter } as any;
    filterToSend.fromDate = this.filter.fromDate ? this.formatDate(this.filter.fromDate) : null;
    filterToSend.toDate = this.filter.toDate ? this.formatDate(this.filter.toDate) : null;

    this.isSubmit = false;

    const filter = {
      WarehouseCode: this.selectedValue,
      Time: this.date?.toISOString()
    };

    this._service.getBaoCaoChiTietXe(filter).subscribe({
      next: (data) => {
        this.lstData = data;

        // Load Google Chart và vẽ biểu đồ
        google.charts.load('current', { packages: ['corechart', 'line'] });  // Đổi sang corechart
        google.charts.setOnLoadCallback(() => this.drawChart());
      },
      error: (response) => {
        console.error('Lỗi khi lấy dữ liệu:', response, filterToSend);
      },
    });
  }

  drawChart(): void {
    const data = new google.visualization.DataTable();
    data.addColumn('number', 'Giờ');
    data.addColumn('number', 'Xe ra');
    data.addColumn('number', 'Xe vào');
    data.addColumn('number', 'Xe không hợp lệ');
  
    const chartData: [number, number, number, number][] = [];
  
    const dataMap = new Map<number, any>();
    this.lstData.forEach(item => {
      dataMap.set(Number(item.hour), item);
    });
  
    let maxValue = 0;

  for (let hour = 0; hour <= 23; hour++) {
    const item = dataMap.get(hour);
    const xeRa = item ? Number(item.xeRa) : 0;
    const xeVao = item ? Number(item.xeVao) : 0;
    const khongHopLe = item ? Number(item.xeKhongHopLe) : 0;

    chartData.push([hour, xeRa, xeVao, khongHopLe]);

    // Tìm max trong 3 loại
    maxValue = Math.max(maxValue, xeRa, xeVao, khongHopLe);
  }

  // Tạo mảng ticks: [0, 1, 2, ..., maxValue]
  const ticks: number[] = [];
  for (let i = 0; i <= maxValue; i++) {
    ticks.push(i);
  }

    
    data.addRows(chartData);
  
    const options = {
      title: 'Số lượng xe theo từng loại trong ngày',
      hAxis: {
        ticks: Array.from({ length: 24 }, (_, i) => ({ v: i, f: i })),
        textStyle: { fontSize: 12 },
        showTextEvery: 1,
        maxAlternation: 1,
        maxTextLines: 1,
      },
      vAxis: {
        minValue: 0,
        textStyle: { fontSize: 12 },
        ticks: ticks
      },
      legend: { position: 'top' }, 
      series: {
        0: { color: 'red' },    // Xe ra
        1: { color: 'blue' },   // Xe vào
        2: { color: 'orange' }  // Không hợp lệ
      }
    };
  
    const chart = new google.visualization.LineChart(document.getElementById('lineChart'));
    chart.draw(data, options);
  }
  
  
  
  formatDate(date: Date): string {
    if (!date) return '';
    const year = date.getFullYear();
    const month = (date.getMonth() + 1).toString().padStart(2, '0');
    const day = date.getDate().toString().padStart(2, '0');
    return `${year}-${month}-${day}`;
  }

  disabledFromDate = (current: Date): boolean => {
    if (this.filter.toDate) {
      return current > this.filter.toDate;
    }
    return false;
  };

  pageSizeChange(size: number): void {
    this.filter.currentPage = 1;
    this.filter.pageSize = size;
    this.search();
  }

  pageIndexChange(index: number): void {
    this.filter.currentPage = index;
    this.search();
  }

  onChange(result: Date): void {
    console.log('onChange: ', result);
  }
  
}
