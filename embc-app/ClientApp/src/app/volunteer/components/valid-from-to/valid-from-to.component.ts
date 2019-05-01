import { Component, EventEmitter, Input, Output } from '@angular/core';
import range from 'lodash/range';

@Component({
  selector: 'app-valid-from-to',
  templateUrl: './valid-from-to.component.html',
  styleUrls: ['./valid-from-to.component.scss']
})
export class ValidFromToComponent {
  @Output() dateStub = new EventEmitter<string>();
  days = range(1, 6); // [1,2,3,4,5]

  constructor() { }

  emitDateStub() {
    this.dateStub.emit('Doom doom!');
  }
}
