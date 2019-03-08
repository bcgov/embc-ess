import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-volunteer-login',
  templateUrl: './volunteer-login.component.html',
  styleUrls: ['./volunteer-login.component.scss']
})
export class VolunteerLoginComponent implements OnInit {

  constructor(private router: Router) { }

  ngOnInit() {
  }
  continue(){
    //this is a simple passthrough for redirecting from the dummy page.
    alert("Routing home.");
    this.router.navigateByUrl('/');

  }
}
