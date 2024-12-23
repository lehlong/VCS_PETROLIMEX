import { Component } from '@angular/core';
import { ShareModule } from '../../shared/share-module';

@Component({
  selector: 'app-order-display',
  standalone: true,
  imports: [ShareModule],
  templateUrl: './order-display.component.html',
  styleUrl: './order-display.component.scss'
})
export class OrderDisplayComponent {
  isFullscreen: boolean = false;
  toggleFullscreen(check : boolean) {
    this.isFullscreen = check;
  }
}
