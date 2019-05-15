import { Component, OnInit, Input, Output, EventEmitter, OnDestroy, forwardRef } from '@angular/core';
import { NgbModal, ModalDismissReasons } from '@ng-bootstrap/ng-bootstrap';
import { Evacuee } from 'src/app/core/models';
import { ValueAccessorBase } from 'src/app/shared/components/value-accessor';
import { NG_VALUE_ACCESSOR } from '@angular/forms';

// tslint:disable: no-use-before-declare
const EVACUEE_LIST_PROVIDER = {
  provide: NG_VALUE_ACCESSOR,
  useExisting: forwardRef(() => EvacueeListComponent),
  multi: true
};

interface EvacueeSelector {
  evacuee: Evacuee;
  selected: boolean;
}

@Component({
  selector: 'app-evacuee-list',
  templateUrl: './evacuee-list.component.html',
  styleUrls: ['./evacuee-list.component.scss'],
  providers: [EVACUEE_LIST_PROVIDER]
})
export class EvacueeListComponent extends ValueAccessorBase<EvacueeSelector[]> implements OnInit, OnDestroy {
  @Output() evacueesChange = new EventEmitter<EvacueeSelector[]>();

  closeResult: string;

  constructor(
    private modalService: NgbModal
  ) {
    super();
  }

  ngOnInit() {
  }
  ngOnDestroy() {
  }

  selectAll() {
    // select all evacuees
    this.value = this.value.map(evacuee => {
      evacuee.selected = true;
      return evacuee;
    });
  }

  emitList() {
    // output the modified list
    this.evacueesChange.emit(this.value);
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
