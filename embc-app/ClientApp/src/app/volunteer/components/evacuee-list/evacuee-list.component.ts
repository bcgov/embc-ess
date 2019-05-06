import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Evacuee } from 'src/app/core/models';

interface EvacueeSelector {
  evacuee: Evacuee;
  selected: boolean;
}
@Component({
  selector: 'app-evacuee-list',
  templateUrl: './evacuee-list.component.html',
  styleUrls: ['./evacuee-list.component.scss']
})
export class EvacueeListComponent implements OnInit {
  @Input() evacuees: EvacueeSelector[];
  @Output() evacueesChange = new EventEmitter<EvacueeSelector[]>();

  constructor() { }

  ngOnInit() {
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
}
