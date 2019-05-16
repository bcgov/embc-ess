import { Component, Input, Output, EventEmitter, forwardRef } from '@angular/core';
import { NgbModal, ModalDismissReasons } from '@ng-bootstrap/ng-bootstrap';
import { Evacuee } from 'src/app/core/models';
import { ValueAccessorBase } from 'src/app/shared/components/value-accessor';
import { NG_VALUE_ACCESSOR } from '@angular/forms';

// poor man's uuid
let identifier = 0;

@Component({
  selector: 'app-evacuee-list',
  templateUrl: './evacuee-list.component.html',
  styleUrls: ['./evacuee-list.component.scss']
})
export class EvacueeListComponent {

  closeResult: string;

  identifier = `evacuee-list-${identifier++}`;

  touched = false;

  // Sub-set of evacuees that have been selected through the UI (i.e. via checkboxes)
  @Input() selected: Evacuee[];

  // List of all evacuees that we want to show in this component
  @Input() evacuees: Evacuee[];

  // Event emitted when an evacuee is selected (or unselected)
  @Output() select = new EventEmitter<Evacuee>();

  @Output() selectAll = new EventEmitter<any>();

  constructor(
    private modalService: NgbModal
  ) { }

  exists(value: Evacuee) {
    if (!this.selected) { return false; }
    return this.selected.some(x => x.id === value.id);
  }

  isSelected(value: Evacuee) {
    return this.exists(value);
  }

  onSelect(value: Evacuee) {
    this.touched = true;
    this.select.emit(value);
  }

  onSelectAll() {
    this.touched = true;
    this.selectAll.emit();
  }

  open(content) {
    this.modalService.open(content, { size: 'lg', ariaLabelledBy: 'modal-basic-title' }).result.then((result) => {
      this.closeResult = `Closed with: ${result}`;
    }, (reason) => {
      this.closeResult = `Dismissed ${this.getDismissReason(reason)}`;
    });
  }

  private getDismissReason(reason: any): string {
    if (reason === ModalDismissReasons.ESC) {
      return 'by pressing ESC';
    } else if (reason === ModalDismissReasons.BACKDROP_CLICK) {
      return 'by clicking on a backdrop';
    } else {
      return `with: ${reason}`;
    }
  }
}
