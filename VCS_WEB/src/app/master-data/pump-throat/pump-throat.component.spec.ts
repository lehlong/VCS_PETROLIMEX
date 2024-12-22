import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PumpThroatComponent } from './pump-throat.component';

describe('PumpThroatComponent', () => {
  let component: PumpThroatComponent;
  let fixture: ComponentFixture<PumpThroatComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PumpThroatComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(PumpThroatComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
