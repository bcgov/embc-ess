import { Component, Input, Output, EventEmitter } from '@angular/core';
import { ValidationErrors } from '@angular/forms';
import { NgbModal, ModalDismissReasons } from '@ng-bootstrap/ng-bootstrap';

import { Evacuee } from 'src/app/core/models';

// poor man's uuid
let identifier = 0;

@Component({
  selector: 'app-evacuee-picker',
  templateUrl: './evacuee-picker.component.html',
  styleUrls: ['./evacuee-picker.component.scss']
})
export class EvacueePickerComponent {

  closeResult: string;

  identifier = `evacuee-list-${identifier++}`;

  touched = false;

  @Input() showErrorsWhen = true;
  @Input() errors: ValidationErrors | null = null;

  // Sub-set of evacuees that have been selected through the UI (i.e. via checkboxes)
  @Input() selected: Evacuee[];

  // List of all evacuees that we want to show in this component
  @Input() evacuees: Evacuee[];

  @Output() select = new EventEmitter<Evacuee>();
  @Output() selectAll = new EventEmitter<any>();

  constructor() { }

  get invalid(): boolean {
    return this.errors && this.showErrorsWhen;
  }

  get valid(): boolean {
    return !this.invalid;
  }

  exists(value: Evacuee) {
    return this.selected && this.selected.some(x => x.id === value.id);
  }

  isSelected(value: Evacuee) {
    return this.exists(value);
  }

  selectEvacuee(value: Evacuee) {
    this.touched = true;
    this.select.emit(value);
  }

  selectAllEvacuees() {
    this.touched = true;
    this.selectAll.emit();
  }
}
