import { Component, Input } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { UniqueKeyService } from 'src/app/core/services/unique-key.service';

@Component({
  selector: 'app-side-box',
  templateUrl: './side-box.component.html',
  styleUrls: ['./side-box.component.scss']
})
export class SideBoxComponent {
  @Input() title = 'TITLE';
  @Input() content = 'Content can be added using the content input.';
  @Input() button = 'Ok';
  @Input() route = '/'; // home by default
  @Input() absoluteRouting = true;

  constructor(
    private router: Router,
    private current: ActivatedRoute,
    private uniqueKeyService: UniqueKeyService,
  ) { }

  continue() {
    // new link is clicked. Clear the unique key for lookup.
    this.uniqueKeyService.clearKey();

    // send the user along to the next page submitted
    if (this.absoluteRouting) {
      this.router.navigateByUrl(this.route);
    } else {
      this.router.navigate([this.route], { relativeTo: this.current });
    }
  }
}
