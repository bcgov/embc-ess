import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-task-number-maker-page',
  templateUrl: './task-number-maker-page.component.html',
  styleUrls: ['./task-number-maker-page.component.scss']
})
export class TaskNumberMakerPageComponent implements OnInit {

  makerMode: boolean = true;
  constructor() { }

  ngOnInit() {
  }

  makerModeChange(onDetails: boolean) {
    this.makerMode = onDetails;
  }

}
