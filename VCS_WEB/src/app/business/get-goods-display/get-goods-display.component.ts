import { Component, OnInit } from '@angular/core';
import { ShareModule } from '../../shared/share-module';

@Component({
  selector: 'app-get-goods-display',
  standalone: true,
  imports: [ShareModule],
  templateUrl: './get-goods-display.component.html',
  styleUrl: './get-goods-display.component.scss'
})
export class GetGoodsDisplayComponent implements OnInit{
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
    if(check == true){
      // this.isZoom = true
      document.documentElement.requestFullscreen()
    }else{
      document.exitFullscreen()
        .then(() => {
      })
        .catch(() => {
      })
    }
  }
}
