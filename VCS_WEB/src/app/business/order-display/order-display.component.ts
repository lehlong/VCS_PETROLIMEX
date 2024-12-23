import { Component, OnInit } from '@angular/core';
import { ShareModule } from '../../shared/share-module';

@Component({
  selector: 'app-order-display',
  standalone: true,
  imports: [ShareModule],
  templateUrl: './order-display.component.html',
  styleUrl: './order-display.component.scss'
})
export class OrderDisplayComponent implements OnInit {
  isFullscreen: boolean = false;


  ngOnInit(){
this.speechNotify();
  }
  speechNotify() : void{
    const utterance = new SpeechSynthesisUtterance('Xin chào! Đây là bản văn bản chuyển đổi thành giọng nói.')
    utterance.lang = 'vi-VN';
    window.speechSynthesis.speak(utterance);
  }
  toggleFullscreen(check : boolean) {
    this.isFullscreen = check;
  }
}
