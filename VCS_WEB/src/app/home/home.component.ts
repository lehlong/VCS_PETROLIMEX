import { Component, OnInit } from '@angular/core'
import { ShareModule } from '../shared/share-module';
declare var google: any
@Component({
  selector: 'app-home',
  standalone: true,
  imports: [ShareModule],
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss',
})
export class HomeComponent implements OnInit {
  ngOnInit(): void {
   
  }
}
