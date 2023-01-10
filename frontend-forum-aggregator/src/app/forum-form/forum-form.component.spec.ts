import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ForumFormComponent } from './forum-form.component';

describe('ForumFormComponent', () => {
  let component: ForumFormComponent;
  let fixture: ComponentFixture<ForumFormComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ForumFormComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ForumFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
