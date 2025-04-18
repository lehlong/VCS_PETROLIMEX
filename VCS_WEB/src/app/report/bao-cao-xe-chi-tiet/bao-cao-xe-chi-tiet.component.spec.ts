import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BaoCaoXeChiTietComponent } from './bao-cao-xe-chi-tiet.component';

describe('BaoCaoXeChiTietComponent', () => {
  let component: BaoCaoXeChiTietComponent;
  let fixture: ComponentFixture<BaoCaoXeChiTietComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [BaoCaoXeChiTietComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(BaoCaoXeChiTietComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
