import { Component, Input, Output, EventEmitter, OnDestroy } from '@angular/core';
import { ReferralType } from 'app/core/models';
import { NgbModalRef, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ConfirmModalComponent } from 'app/shared/modals/confirm/confirm.component';

@Component({
  selector: 'app-referral-list-item',
  templateUrl: './referral-list-item.component.html',
  styleUrls: ['./referral-list-item.component.scss']
})
export class ReferralListItemComponent implements OnDestroy {
  @Input() type: ReferralType;
  @Output() remove = new EventEmitter<any>();
  @Output() add = new EventEmitter<any>();

  private modalRef: NgbModalRef = null;

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

  constructor(private modals: NgbModal) { }

  ngOnDestroy() {
    // close modal if it's open
    if (this.modalRef) { this.modalRef.dismiss(); }
  }

  confirm() {
    // confirm before removing a referral
    this.modalRef = this.modals.open(ConfirmModalComponent, { centered: true });
    this.modalRef.componentInstance.message = 'Are you sure you want to remove this referral?';
    this.modalRef.result.then(
      () => {
        this.modalRef = null;
        this.onRemove();
      },
      () => { this.modalRef = null; }
    );
  }
}
