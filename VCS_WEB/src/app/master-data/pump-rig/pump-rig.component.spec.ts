import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PumpRigComponent } from './pump-rig.component';

describe('PumpRigComponent', () => {
  let component: PumpRigComponent;
  let fixture: ComponentFixture<PumpRigComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PumpRigComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(PumpRigComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
