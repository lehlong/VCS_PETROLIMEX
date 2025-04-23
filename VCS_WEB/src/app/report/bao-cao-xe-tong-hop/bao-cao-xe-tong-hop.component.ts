import { Component } from '@angular/core';
import { HeaderFilter } from '../../models/bussiness/header.model';
import { PaginationResult } from '../../models/base.model';
import { ShareModule } from '../../shared/share-module';
import { ReportService } from '../../services/report/report.service';
import { GlobalService } from '../../services/global.service';
import { WarehouseService } from '../../services/master-data/warehouse.service';
import { HeaderService } from '../../services/business/header.service';
import { ReportModel } from '../../models/bussiness/report.model';

declare const google: any; // Khai báo biến global từ google chart

@Component({
  selector: 'app-bao-cao-xe-tong-hop',
  standalone: true,
  imports: [ShareModule],
  templateUrl: './bao-cao-xe-tong-hop.component.html',
  styleUrl: './bao-cao-xe-tong-hop.component.scss'
})
export class BaoCaoXeTongHopComponent {
  filter = new ReportModel();
  paginationResult = new PaginationResult();
  isSubmit: boolean = false;
  loading: boolean = false;
  lstData: any[] = [];
  selectedValue = '';
  tDate: Date | null = null;
  fDate: Date | null = null;
  lstWareHouse: any[] = []
  lstBcXeTongHop: any[] = []
  WareHouse: any
  companyCode?: string = localStorage.getItem('companyCode')?.toString()

  constructor(
    private _service: ReportService,
    private _serviceHeader: HeaderService,
    private _WarehouseService: WarehouseService,
    private globalService: GlobalService,
  ) {
    this.globalService.setBreadcrumb([
      {
        name: 'Báo cáo xe tổng hợp',
        path: 'report/bao-cao-xe-tong-hop',
      },
    ]);
    this.globalService.getLoading().subscribe((value) => {
      this.loading = value;
    });
  }

  ngOnInit(): void {
    this.getWarehouse();
    this.getLstBaoCao();
    this.fDate = new Date();
  }

  getWarehouse() {
    this._WarehouseService.getByOrg(this.companyCode).subscribe({
      next: (data) => {
        this.lstWareHouse = data;
      },
      error: (response) => {
        console.error(response);
      },
    });
  }

  getLstBaoCao() {
    this.filter = {
      warehouseCode: this.selectedValue,
      fDate: this.fDate ? this.formatDate(this.fDate) : null,
      tDate: this.tDate ? this.formatDate(this.tDate) : null
    } as ReportModel;
    this._service.baoCaoXeTongHop(this.filter).subscribe({

      next: (data) => {
        this.lstData = data;
        console.log(this.lstData);

        // Load Google Chart và vẽ biểu đồ
        google.charts.load('current', { packages: ['corechart', 'line'] });  // Đổi sang corechart
        google.charts.setOnLoadCallback(() => {
          setTimeout(() => {
            this.drawChart()
          }, 5);
        });
      },
      error: (response) => {
        console.error(response);
      },
    });
  }

  reset() {
      this.selectedValue = ""
      this.fDate = null
      this.tDate =  null
    this.getLstBaoCao()
  }
  drawChart(): void {
    const data = new google.visualization.DataTable();
    data.addColumn('number', 'ngày');
    data.addColumn('number', 'Xe ra');
    data.addColumn('number', 'Xe vào');
    data.addColumn('number', 'Xe không hợp lệ');

    const chartData: [number, number, number, number][] = [];

    const dataMap = new Map<number, any>();
    this.lstData.forEach(item => {
      dataMap.set(Number(item.date), item);


    });

    let maxValue = 0;

this.lstData.forEach(e => {
  const item = dataMap.get(e.date);

  const xeRa = item ? Number(item.xeRa) : 0;
  const xeVao = item ? Number(item.xeVao) : 0;
  const khongHopLe = item ? Number(item.xeKhongHopLe) : 0;

  chartData.push([e, xeRa, xeVao, khongHopLe]);

  // Tìm max trong 3 loại
  maxValue = Math.max(maxValue, xeRa, xeVao, khongHopLe);
});

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


  onChange(result: Date): void {
    console.log('onChange: ', result);
  }

  downloadFileExcel() {
    const filter: any = {
      Time: this.fDate?.toISOString()
    };
    if (this.selectedValue != null) {
      filter.WarehouseCode = this.selectedValue;
    }
    this._service.ExportExcelBaoCaoXeTongHop(filter).subscribe({
      next: (response) => {
        const blob = new Blob([response], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
        const url = window.URL.createObjectURL(blob);
        const a = document.createElement('a');
        a.href = url;
        a.download = 'BaoCaoXeTongHop.xlsx'; // Hoặc lấy từ header
        document.body.appendChild(a);
        a.click();
        document.body.removeChild(a);
        window.URL.revokeObjectURL(url);
      },
      error: (error) => {
        console.log('Lỗi:', error);
      }
    });
  }
}
