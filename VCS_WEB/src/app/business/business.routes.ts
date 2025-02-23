import { Routes } from '@angular/router'
import { OrderDisplayComponent } from './order-display/order-display.component'
import { GetGoodsDisplayComponent } from './get-goods-display/get-goods-display.component'
import { GetTicketComponent } from './get-ticket/get-ticket.component'
import { OrderDisplayListComponent } from './order-display-list/order-display-list.component'

export const businessRoutes: Routes = [
  { path: 'order-display/:id', component: OrderDisplayComponent },
  { path: 'get-goods-display', component: GetGoodsDisplayComponent },
  { path: 'get-ticket', component: GetTicketComponent },
  { path: 'order-display-list', component: OrderDisplayListComponent },
]
