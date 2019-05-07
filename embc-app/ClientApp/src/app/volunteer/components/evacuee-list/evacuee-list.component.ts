import { Component, OnInit, Input, Output, EventEmitter, OnDestroy } from '@angular/core';
import { Evacuee } from 'src/app/core/models';
import {NgbModal, ModalDismissReasons} from '@ng-bootstrap/ng-bootstrap';

interface EvacueeSelector {
  evacuee: Evacuee;
  selected: boolean;
}
@Component({
  selector: 'app-evacuee-list',
  templateUrl: './evacuee-list.component.html',
  styleUrls: ['./evacuee-list.component.scss']
})
export class EvacueeListComponent implements OnInit, OnDestroy {
  @Input() evacuees: EvacueeSelector[];
  @Output() evacueesChange = new EventEmitter<EvacueeSelector[]>();
  closeResult:string;
  constructor(
    private modalService: NgbModal
  ) { }

  ngOnInit() {
  }
  ngOnDestroy() {
  }

  selectAll() {
    // select all evacuees
    this.evacuees = this.evacuees.map(evacuee => {
      evacuee.selected = true;
      return evacuee;
    });
  }
  emitList() {
    // output the modified list
    this.evacueesChange.emit(this.evacuees);
  }


  open(content) {
    this.modalService.open(content, {size: 'lg', ariaLabelledBy: 'modal-basic-title'}).result.then((result) => {
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
      return  `with: ${reason}`;
    }
  }
}
