import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GetGoodsDisplayComponent } from './get-goods-display.component';

describe('GetGoodsDisplayComponent', () => {
  let component: GetGoodsDisplayComponent;
  let fixture: ComponentFixture<GetGoodsDisplayComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [GetGoodsDisplayComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(GetGoodsDisplayComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
