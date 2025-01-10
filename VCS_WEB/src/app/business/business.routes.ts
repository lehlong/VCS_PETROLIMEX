import { Routes } from '@angular/router'
import { OrderDisplayComponent } from './order-display/order-display.component'
import { GetGoodsDisplayComponent } from './get-goods-display/get-goods-display.component'
import { GetTicketComponent } from './get-ticket/get-ticket.component'

export const businessRoutes: Routes = [
  { path: 'order-display', component: OrderDisplayComponent },
  { path: 'get-goods-display', component: GetGoodsDisplayComponent },
  { path: 'get-ticket', component: GetTicketComponent },
]
