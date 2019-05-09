import { Component, Input, Output, EventEmitter } from '@angular/core';
import { ReferralType } from 'src/app/core/models';

@Component({
  selector: 'app-referral-list-item',
  templateUrl: './referral-list-item.component.html',
  styleUrls: ['./referral-list-item.component.css']
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
        return 'ADD ANOTHER FOOD REFERRAL';
      case 'LODGING':
        return 'ADD ANOTHER LODGING REFERRAL';
      case 'CLOTHING':
        return 'ADD ANOTHER CLOTHING REFERRAL';
      case 'TRANSPORTATION':
        return 'ADD ANOTHER TRANSPORT REFERRAL';
      case 'INCIDENTALS':
      default:
        return 'ADD ANOTHER INCIDENTALS REFERRAL';
    }
  }
}
