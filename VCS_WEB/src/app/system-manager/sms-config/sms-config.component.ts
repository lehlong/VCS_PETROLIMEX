import { Component, OnInit } from '@angular/core';
import { ShareModule } from '../../shared/share-module';
import { SmsConfigService } from '../../services/system-manager/sms-config.service';

@Component({
  selector: 'app-sms-config',
  standalone: true,
  imports: [ShareModule],
  templateUrl: './sms-config.component.html',
  styleUrl: './sms-config.component.scss'
})
export class SmsConfigComponent implements OnInit {
  constructor(private _service: SmsConfigService) {}

  data : any = {}
  ngOnInit(): void {
   this.getSMS(); 
  }
  getSMS(){
    this._service.GetSMS().subscribe({
      next:(data) => {
        this.data = data
      }
    })
  }
  updateSMS(){
    this._service.UpdateSMS(this.data).subscribe({
      next:(data) => {
        this.ngOnInit();
      }
    })
  }
}
