import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BaoCaoSanPhamTongHopComponent } from './bao-cao-san-pham-tong-hop.component';

describe('BaoCaoSanPhamTongHopComponent', () => {
  let component: BaoCaoSanPhamTongHopComponent;
  let fixture: ComponentFixture<BaoCaoSanPhamTongHopComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [BaoCaoSanPhamTongHopComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(BaoCaoSanPhamTongHopComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
