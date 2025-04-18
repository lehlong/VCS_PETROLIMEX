import { Routes } from '@angular/router'
import { BaoCaoXeChiTietComponent } from './bao-cao-xe-chi-tiet/bao-cao-xe-chi-tiet.component'
import { BaoCaoXeTongHopComponent } from './bao-cao-xe-tong-hop/bao-cao-xe-tong-hop.component'
import { BaoCaoSanPhamTongHopComponent } from './bao-cao-san-pham-tong-hop/bao-cao-san-pham-tong-hop.component'

export const reportRoutes: Routes = [
{ path: 'bao-cao-xe-chi-tiet', component: BaoCaoXeChiTietComponent },
{ path: 'bao-cao-xe-tong-hop', component: BaoCaoXeTongHopComponent },
{ path: 'bao-cao-san-pham-tong-hop', component: BaoCaoSanPhamTongHopComponent },
]
