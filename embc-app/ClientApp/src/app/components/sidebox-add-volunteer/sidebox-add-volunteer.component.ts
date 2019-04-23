import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-sidebox-add-volunteer',
  templateUrl: './sidebox-add-volunteer.component.html',
  styleUrls: ['./sidebox-add-volunteer.component.scss']
})
export class SideboxAddVolunteerComponent implements OnInit {
  route = '../volunteer';

  constructor() { }

  ngOnInit() { }

}
