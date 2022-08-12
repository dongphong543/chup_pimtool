import { async, ComponentFixture, TestBed } from "@angular/core/testing";

import { NPSComponent } from "./nps.component";

describe("NPSComponent", () => {
  let component: NPSComponent;
  let fixture: ComponentFixture<NPSComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [NPSComponent],
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NPSComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it("should create", () => {
    expect(component).toBeTruthy();
  });
});
