import { Component, Input, ViewChild } from '@angular/core';
import { FormGroup, FormControl } from '@angular/forms';
import { NgbTypeahead, NgbTypeaheadConfig } from '@ng-bootstrap/ng-bootstrap';
import { Store } from '@ngrx/store';
import { Observable, Subject, merge } from 'rxjs';
import { debounceTime, distinctUntilChanged, map, filter } from 'rxjs/operators';
import { AppState } from 'src/app/store';
import { Community, Organization } from 'src/app/core/models';

//
// ref: https://ng-bootstrap.github.io/#/components/typeahead/
//

@Component({
  selector: 'app-communities-select',
  templateUrl: './communities-select.component.html',
  styleUrls: ['./communities-select.component.scss']
})
export class CommunitiesSelectComponent {
  @Input() myFormControl: Organization = null;
  @Input() myParent: FormGroup = null;
  @Input() myFormControlName: string = null;
  @Input() myRequired = false;
  @Input() myPlaceHolder = 'Type or click to search...';
  @Input() myId: string = null;
  @ViewChild('instance') instance: NgbTypeahead;

  // the observable
  communities$ = this.store.select(s => s.lookups.communities.communities);

  // the current list
  communities: Community[];

  // for event-driven search
  click$ = new Subject<string>();
  focus$ = new Subject<string>(); // for future use

  // formatters
  inputFormatter = (c: Community) => c.name;
  resultFormatter = (c: Community) => c.name;

  // function that returns communities whose name partial-matches search text
  // returns when search text has changed or on click/focus events
  search = (text$: Observable<string>) => {
    const debouncedText$ = text$.pipe(debounceTime(200), distinctUntilChanged());
    const clicksWithClosedPopup$ = this.click$.pipe(filter(() => !this.instance.isPopupOpen()));
    const inputFocus$ = this.focus$;

    return merge(debouncedText$, inputFocus$, clicksWithClosedPopup$).pipe(
      map(term => {
        if (!term) {
          return this.communities; // return all
        } else {
          return this.communities.filter(community => community.name.toLowerCase().indexOf(term.toLowerCase()) > -1);
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
    // TODO: fix this
    // temporary hack to de-serialize communities
    // so we can work with them more easily in the search function
    this.communities$.subscribe(value => this.communities = value);

    // NgbTypeahead config
    this.config.editable = false;
    this.config.focusFirst = false;
    this.config.placement = ['bottom-left'];
  }

  onBlur(search: string) {
    // look for exact match
    const found = this.communities.find(community => community.name.toLowerCase() === search.toLowerCase());

    // if exact match was found, use it
    // otherwise clear control to avoid confusion
    if (this.myParent && this.myFormControlName) {
      this.myParent.controls[this.myFormControlName].setValue(found || null);
    } else if (this.myFormControl) {
      (this.myFormControl as unknown as FormControl).setValue(found || null);
    }
  }
}
