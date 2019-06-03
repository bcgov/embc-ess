import { Component, Input, Output, EventEmitter } from '@angular/core';
import { ReferralType } from 'src/app/core/models';

@Component({
  selector: 'app-referral-list-item',
  templateUrl: './referral-list-item.component.html',
  styleUrls: ['./referral-list-item.component.scss']
})
export class ReferralListItemComponent {
  @Input() type: ReferralType;
  @Output() remove = new EventEmitter<any>();
  @Output() add = new EventEmitter<any>();

  onRemove(): void { this.remove.emit(); }
  onAdd(): void { this.add.emit(); }

  // --------------------HELPERS-----------------------------------------
  get addReferralString(): string {
    switch (this.type) {
      case 'FOOD':
        return 'Add Another Food Referral';
      case 'LODGING':
        return 'Add Another Lodging Referral';
      case 'CLOTHING':
        return 'Add Another Clothing Referral';
      case 'TRANSPORTATION':
        return 'Add Another Transport Referral';
      case 'INCIDENTALS':
        return 'Add Another Incidentals Referral';
      default:
        return 'Add Another Referral'; // should never happen
    }
  }
}
