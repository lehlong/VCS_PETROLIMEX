import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BaoCaoXeTongHopComponent } from './bao-cao-xe-tong-hop.component';

describe('BaoCaoXeTongHopComponent', () => {
  let component: BaoCaoXeTongHopComponent;
  let fixture: ComponentFixture<BaoCaoXeTongHopComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [BaoCaoXeTongHopComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(BaoCaoXeTongHopComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
