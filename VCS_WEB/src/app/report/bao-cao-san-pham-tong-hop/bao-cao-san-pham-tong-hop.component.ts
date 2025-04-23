import { Component } from '@angular/core';
import { HeaderFilter } from '../../models/bussiness/header.model';
import { PaginationResult } from '../../models/base.model';
import { ShareModule } from '../../shared/share-module';
import { ReportService } from '../../services/report/report.service';
import { GlobalService } from '../../services/global.service';
import { WarehouseService } from '../../services/master-data/warehouse.service';
import { ReportModel } from '../../models/bussiness/report.model';
import { HeaderService } from '../../services/business/header.service';
import { GoodsService } from '../../services/master-data/goods.service';

declare const google: any; // Khai báo biến global từ google chart
@Component({
  selector: 'app-bao-cao-san-pham-tong-hop',
  standalone: true,
  imports: [ShareModule],
  templateUrl: './bao-cao-san-pham-tong-hop.component.html',
  styleUrl: './bao-cao-san-pham-tong-hop.component.scss'
})
export class BaoCaoSanPhamTongHopComponent {
  filter = new ReportModel();
  paginationResult = new PaginationResult();
  isSubmit: boolean = false;
  loading: boolean = false;
  lstData: any[] = [];
  selectedValue = '';
  tDate: Date | null = null;
  fDate: Date | null = null;
  lstWareHouse: any[] = []
  lstGoods: any[] = []
  WareHouse: any
  companyCode?: string = localStorage.getItem('companyCode')?.toString()
  constructor(
    private _service: ReportService,
    private _serviceHeader: HeaderService,
    private _serviceGoods: GoodsService,
    private _WarehouseService: WarehouseService,
    private globalService: GlobalService,
  ) {
    this.globalService.setBreadcrumb([
      {
        name: 'Báo cáo sản phẩm tổng hợp',
        path: 'report/bao-cao-san-pham-tong-hop',
      },
    ]);
    this.globalService.getLoading().subscribe((value) => {
      this.loading = value;
    });
  }

  ngOnInit(): void {
    this.getLstBaoCao();
    this.getWarehouse();
    this.getGoods();
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
  getGoods() {
    this._serviceGoods.getall().subscribe({
      next: (data) => {
        this.lstGoods = data;
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
    this._service.baoCaoSanPhamTongHop(this.filter).subscribe({

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

  drawChart(): void {
    const data = new google.visualization.DataTable();
    data.addColumn('number', 'Giờ');
    this.lstGoods.forEach(e => {
      data.addColumn('number', e.name);

    });

    const chartData: (number | null)[][] = []; // Mảng động

    const dataMap = new Map<number, any>();
    this.lstData.forEach(item => {
      dataMap.set(Number(item.hour), item);
    });

    let maxValue = 0;
    this.lstData.forEach(e => {
      const item = dataMap.get(e.date);
      const rowData: (number | null)[] = [e.date]; // Khởi tạo hàng với giờ

      // Thêm các giá trị cho từng loại hàng hóa
      this.lstGoods.forEach(g => {

          const goodsValue = item?.priceGoods.find((p: { goodsCode: any; }) => p.goodsCode === g.code)?.price || 0;

          rowData.push(goodsValue);
          maxValue = Math.max(maxValue, goodsValue); // Cập nhật maxValue

      });

      chartData.push(rowData);
    });
    // for (let hour = 0; hour <= 23; hour++) {
    //   const item = dataMap.get(hour);
    //   const rowData: (number | null)[] = [hour]; // Khởi tạo hàng với giờ

    //   // Thêm các giá trị cho từng loại hàng hóa
    //   this.lstGoods.forEach(e => {
    //     const goodsValue = item?.priceGoods.find((g: { goodsCode: any; }) => g.goodsCode === e.goodsCode)?.price || 0;
    //     rowData.push(goodsValue);
    //     maxValue = Math.max(maxValue, goodsValue); // Cập nhật maxValue
    //   });

    //   chartData.push(rowData);
    //   // Tìm max trong 3 loại
    //   // maxValue = Math.max(maxValue, xeVao, xeRa, khongHopLe);
    // }

    // Tạo mảng ticks: [0, 1, 2, ..., maxValue]
    const ticks: number[] = [];
    for (let i = 0; i <= maxValue; i++) {
      ticks.push(i);
    }


    data.addRows(chartData);

    const options = {
      title: 'Số lượng xe theo từng loại trong ngày',
      hAxis: {
          ticks: Array.from({ length: 24 }, (_, i) => ({ v: i, f: `${i}` })),
          textStyle: { fontSize: 12 },
          showTextEvery: 1,
          maxAlternation: 1,
          maxTextLines: 1,
      },
      vAxis: {
          minValue: 0,
          textStyle: { fontSize: 12 },
          ticks: Array.from({ length: maxValue + 1 }, (_, i) => i),
      },
      legend: {
          position: 'right', // Di chuyển legend sang bên phải
          alignment: 'center' // Căn giữa
      },
      series: {
          0: { color: 'green' },
          1: { color: 'red' },
          2: { color: 'blue' }
      }
  };

    const chart = new google.visualization.LineChart(document.getElementById('lineChart'));
    chart.draw(data, { ...options, width: 800, height: 400 });
  }



  formatDate(date: Date): string {
    if (!date) return '';
    const year = date.getFullYear();
    const month = (date.getMonth() + 1).toString().padStart(2, '0');
    const day = date.getDate().toString().padStart(2, '0');
    return `${year}-${month}-${day}`;
  }

  // disabledFromDate = (current: Date): boolean => {
  //   if (this.filter.toDate) {
  //     return current > this.filter.toDate;
  //   }
  //   return false;
  // };

  // pageSizeChange(size: number): void {
  //   this.filter.currentPage = 1;
  //   this.filter.pageSize = size;
  //   this.search();
  // }

  // pageIndexChange(index: number): void {
  //   this.filter.currentPage = index;
  //   this.search();
  // }

  onChange(result: Date): void {
    console.log('onChange: ', result);
  }

  onTabChange(index: number): void {
    if (index === 1) {
      // Đảm bảo google charts được load rồi mới gọi drawChart
      google.charts.load('current', { packages: ['corechart', 'line'] });

      google.charts.setOnLoadCallback(() => {
        setTimeout(() => {
          this.drawChart();
        }, 0); // đợi DOM render xong
      });
    }
  }

  downloadFileExcel() {
    const filter: any = {
      fDate: this.fDate?.toISOString(),
      tDate: this.tDate?.toISOString() ?? null
    };
    if (this.selectedValue != null) {
      filter.WarehouseCode = this.selectedValue;
    }
    this._service.ExportExcelBaoCaoSanPhamTongHop(filter).subscribe({
      next: (response) => {
        // Tạo blob
        const blob = new Blob([response], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });

        // Tạo URL
        const url = window.URL.createObjectURL(blob);

        // Tạo link
        const a = document.createElement('a');
        a.href = url;
        a.download = 'BaoCaoSanPhamTongHop.xlsx'; // Hoặc lấy từ header
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
