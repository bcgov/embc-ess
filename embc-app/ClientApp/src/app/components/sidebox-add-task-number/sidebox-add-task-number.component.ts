import { Component, OnInit } from '@angular/core';
import { AuthService } from 'src/app/core/services/auth.service';
import { UniqueKeyService } from 'src/app/core/services/unique-key.service';

@Component({
  selector: 'app-sidebox-add-task-number',
  templateUrl: './sidebox-add-task-number.component.html',
  styleUrls: ['./sidebox-add-task-number.component.scss']
})
export class SideboxAddTaskNumberComponent implements OnInit {

  route: string;
  constructor(
    private authService: AuthService,
  ) { }

  ngOnInit() {
    this.authService.path.subscribe(p => {
      this.route = `/${p}/task-number`;
    });
  }

}
