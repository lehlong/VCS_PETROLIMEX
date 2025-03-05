import { Component, OnInit } from '@angular/core';
import { ShareModule } from '../../../shared/share-module';

@Component({
  selector: 'app-history-detail',
  standalone: true,
  imports: [ShareModule],
  templateUrl: './history-detail.component.html',
  styleUrl: './history-detail.component.scss'
})
export class HistoryDetailComponent implements OnInit {




  
  ngOnInit() {
    
  }

}
