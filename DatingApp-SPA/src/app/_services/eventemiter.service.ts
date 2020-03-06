import { Injectable, EventEmitter } from '@angular/core';
import { Subscription } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class EventemiterService {

invokeFunction = new EventEmitter();
subscription: Subscription;

constructor() { }

onSearchButtonClick(username: string) {
  this.invokeFunction.emit(username);
}

}
