import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-organization-maker-page',
  templateUrl: './organization-maker-page.component.html',
  styleUrls: ['./organization-maker-page.component.scss']
})
export class OrganizationMakerPageComponent implements OnInit {
  editMode: boolean = null;
  constructor() { }
  get usefulInformationText(): string {
    return this.editMode
            ? "Did you know that each organization will have a Business Accounts Manager who manages all aspects of the Business BCeID such as adding and removing users, resetting passwords and more."
            : "Please contact BCeID Help Desk for support at 1-888-356-2741 for any Business BCeID related queries.";
  }
  ngOnInit() {
  }

  editModeChange(onEditMode: boolean) {
    this.editMode = onEditMode;
  }

}
