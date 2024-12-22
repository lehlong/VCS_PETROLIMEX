import { Routes } from '@angular/router'
import { CurrencyComponent } from './currency/currency.component'
import { UnitComponent } from './unit/unit.component'
import { LocalComponent } from './local/local.component'
import { AreaComponent } from './area/area.component'
import { PeriodTimeComponent } from './period-time/period-time.component'
import { AccountTypeComponent } from './account-type/account-type.component'
import { MarketComponent } from './market/market.component'
import { GoodsComponent } from './goods/goods.component'
import { CustomerComponent } from './customer/customer.component'
import { DeliveryPointComponent } from './delivery-point/delivery-point.component'
import { CustomerTypeComponent } from './customer-type/customer-type.component'
import { WarehouseComponent } from './warehouse/warehouse.component'
import { MarketCompetitorComponent } from './market-competitor/market-competitor.component'
import { PumpRigComponent } from './pump-rig/pump-rig.component'
import { PumpThroatComponent } from './pump-throat/pump-throat.component'
export const masterDataRoutes: Routes = [
  { path: 'currency', component: CurrencyComponent },
  { path: 'unit', component: UnitComponent },
  { path: 'local', component: LocalComponent },
  { path: 'area', component: AreaComponent },
  { path: 'audit-year', component: PeriodTimeComponent },
  { path: 'account-type', component: AccountTypeComponent },

  { path: 'goods', component: GoodsComponent, },
  { path: 'market', component: MarketComponent, },
  { path: 'customer', component: CustomerComponent },
  { path: 'delivery-point', component: DeliveryPointComponent },
  { path: 'customer-type', component: CustomerTypeComponent },
  { path: 'warehouse', component: WarehouseComponent },
  { path: 'market-competitor', component: MarketCompetitorComponent },
  { path: 'pump-rig', component: PumpRigComponent },
  { path: 'pump-throat', component: PumpThroatComponent },

]
