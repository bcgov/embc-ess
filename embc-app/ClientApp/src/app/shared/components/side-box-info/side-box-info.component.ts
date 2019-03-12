import { Component, OnInit, Input } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-side-box-info',
  templateUrl: './side-box-info.component.html',
  styleUrls: ['./side-box-info.component.scss']
})
export class SideBoxInfoComponent implements OnInit {
  @Input() title = 'TITLE';
  @Input() content = 'Content can be added using the content input.';
  @Input() button = 'Ok';
  @Input() route = '/'; //home by default


  constructor(private router: Router) { }

  ngOnInit() {
  }

  continue(){
    //send the user along to the next page submitted
    this.router.navigateByUrl(this.route);
  }
}
