import { Component } from '@angular/core'
import { ShareModule } from '../../shared/share-module'
@Component({
  selector: 'app-get-ticket',
  standalone: true,
  imports: [ShareModule],
  templateUrl: './get-ticket.component.html',
  styleUrl: './get-ticket.component.scss',
})
export class GetTicketComponent {}
