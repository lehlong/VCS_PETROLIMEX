import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OrderDisplayListComponent } from './order-display-list.component';

describe('OrderDisplayListComponent', () => {
  let component: OrderDisplayListComponent;
  let fixture: ComponentFixture<OrderDisplayListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [OrderDisplayListComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(OrderDisplayListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
