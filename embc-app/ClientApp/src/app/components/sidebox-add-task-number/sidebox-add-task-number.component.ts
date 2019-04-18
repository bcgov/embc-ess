import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-sidebox-add-task-number',
  templateUrl: './sidebox-add-task-number.component.html',
  styleUrls: ['./sidebox-add-task-number.component.scss']
})
export class SideboxAddTaskNumberComponent implements OnInit {

  route = '../task-number';
  constructor() { }

  ngOnInit() {
  }

}
