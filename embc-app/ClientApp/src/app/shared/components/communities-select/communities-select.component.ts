import { Component, Input, ViewChild } from '@angular/core';
import { FormGroup, FormControl } from '@angular/forms';
import { NgbTypeahead, NgbTypeaheadConfig } from '@ng-bootstrap/ng-bootstrap';
import { Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import { debounceTime, distinctUntilChanged, map } from 'rxjs/operators';
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
        if (term.length < 3) {
          return [];
        } else {
          const tlc = term.toLowerCase();
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

  onBlur(search: string) {
    // look for exact match
    const found = this.communities.find(community => (community.name.toLowerCase() === search.toLowerCase()));

    // if exact match was found, use it
    // otherwise clear control to avoid user confusion
    if (this.myParent && this.myFormControlName) {
      this.myParent.controls[this.myFormControlName].setValue(found || null);
    } else if (this.myFormControl) {
      (this.myFormControl as unknown as FormControl).setValue(found || null);
    }
  }
}
