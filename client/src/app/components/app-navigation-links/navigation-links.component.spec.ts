import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AppNavigationLinksComponent } from './navigation-links.component';

describe('AppNavigationLinksComponent', () => {
  let component: AppNavigationLinksComponent;
  let fixture: ComponentFixture<AppNavigationLinksComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AppNavigationLinksComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(AppNavigationLinksComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
