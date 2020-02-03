import { Component, OnInit, Input, Output } from '@angular/core';
import { ListResult, EvacueeListItem } from 'src/app/core/models';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/core/services/auth.service';
import { UniqueKeyService } from 'src/app/core/services/unique-key.service';

@Component({
  selector: 'app-evacuee-search-results',
  templateUrl: './evacuee-search-results.component.html',
  styleUrls: ['./evacuee-search-results.component.scss']
})
export class EvacueeSearchResultsComponent implements OnInit {
  @Input() searchResults: ListResult<EvacueeListItem>;
  @Output() displayResults: boolean = true;
  path: string = null; // the base path for routing

  constructor( 
    private router: Router,
    private authService: AuthService,
    private uniqueKeyService: UniqueKeyService,
    ){ }

  ngOnInit() {
    // save the base url path
    this.authService.path.subscribe((path: string) => this.path = path);
  }

  view(registrationId: string) {
    // save registration ID for lookup in the new component
    this.uniqueKeyService.setKey(registrationId);

    // go to registration summary page
    this.router.navigate([`/${this.path}/registration/summary`]);
  }

  searchAgain() {
    this.displayResults = false;
  }

}
