import { Component, Input, ViewChild } from '@angular/core';
import { FormGroup, FormControl } from '@angular/forms';
import { NgbTypeahead, NgbTypeaheadConfig } from '@ng-bootstrap/ng-bootstrap';
import { Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import { debounceTime, distinctUntilChanged, map } from 'rxjs/operators';
import { AppState } from 'app/store';
import { Community, Organization } from 'app/core/models';

//
// ref: https://ng-bootstrap.github.io/#/components/typeahead/
//

const MIN_SEARCH_LENGTH = 3;

@Component({
  selector: 'app-communities-select',
  templateUrl: './communities-select.component.html',
  styleUrls: ['./communities-select.component.scss']
})
export class CommunitiesSelectComponent {
  @Input() myFormControl: FormControl;
  @Input() myParent: FormGroup = null;
  @Input() myFormControlName: string = null;
  @Input() myRequired = false;
  @Input() myPlaceHolder = 'Type to search...';
  @Input() myId: string = null;
  @ViewChild('instance') instance: NgbTypeahead;

  // the observable
  communities$ = this.store.select(s => s.lookups.communities.communities);

  // the current list
  communities: Array<any> = [];

  // formatters
  inputFormatter = (c: Community) => c.name;
  resultFormatter = (c: Community) => c.name;

  // function that returns communities whose name partial-matches search text
  // returns when search text has changed or on click/focus events
  search = (text$: Observable<string>) => {
    return text$.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      map(term => {
        if (term.length < MIN_SEARCH_LENGTH) {
          return [];
        } else {
          // any apostrophe looking character get replaced.
          // The database should have nothing except for ' so ’ and ‘ should match ' .
          const tlc = term.toLowerCase().replace(/[‘’]/g, '\'');
          return this.communities.filter(community => (community.name.toLowerCase().indexOf(tlc) > -1));
        }
      })
    );
  }

  //
  // TODO: move form validators here?
  //       see address-selector component
  //

  constructor(
    private store: Store<AppState>,
    private config: NgbTypeaheadConfig
  ) {
    // de-serialize communities so we can work with them more easily in the search function
    // first map to smaller objects (for theoretically better performance)
    this.communities$
      .pipe(map(x => x.map(y => ({ id: y.id, name: y.name }))))
      .subscribe(value => this.communities = value);

    // NgbTypeahead config
    this.config.editable = false;
    this.config.focusFirst = false;
    this.config.placement = ['bottom-left'];
  }

  onBlur(term: string) {
    let value = null;

    // ensure we have a large enough search term
    if (term && term.length >= MIN_SEARCH_LENGTH) {
      // prepare for case-insensitive search and replace weird quotes with regular quotes
      term = term.toLowerCase().replace(/[‘’]/g, '\'');

      // first look for an exact match
      const exact = this.communities.find(c => c.name.toLowerCase() === term);
      if (exact) {
        value = exact;
      } else {
        // otherwise look for a single partial match
        const partials = this.communities.filter(c => c.name.toLowerCase().includes(term));
        if (partials.length === 1) {
          value = partials[0];
        }
      }
    }

    // if a match was found, use it
    // otherwise clear control to avoid user confusion
    if (this.myParent && this.myFormControlName) {
      this.myParent.controls[this.myFormControlName].setValue(value);
    } else if (this.myFormControl) {
      this.myFormControl.setValue(value);
    }
  }
}
