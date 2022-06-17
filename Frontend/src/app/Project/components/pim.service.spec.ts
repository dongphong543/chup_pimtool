import { TestBed } from "@angular/core/testing";

import { PIMService } from "./pim.service";

describe("PIMService", () => {
  let service: PIMService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(PIMService);
  });

  it("should be created", () => {
    expect(service).toBeTruthy();
  });
});
