/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { EventemiterService } from './eventemiter.service';

describe('Service: Eventemiter', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [EventemiterService]
    });
  });

  it('should ...', inject([EventemiterService], (service: EventemiterService) => {
    expect(service).toBeTruthy();
  }));
});
