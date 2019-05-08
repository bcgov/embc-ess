import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Supplier } from 'src/app/core/models';

@Component({
  selector: 'app-supplier',
  templateUrl: './supplier.component.html',
  styleUrls: ['./supplier.component.scss']
})
export class SupplierComponent implements OnInit {
  @Input() supplier: Supplier;
  // tslint:disable-next-line: no-inferrable-types
  @Input() editMode?: boolean = true; // false is read only fields
  @Input() id?: string = 'generic';
  @Output() supplierChange = new EventEmitter<Supplier>();

  validSupplierName = true;
  validSupplierAddress = true;
  validSupplierCity = true;

  constructor() { }

  ngOnInit() { }
  emitSupplier(supplier: Supplier) {
    this.supplierChange.emit(supplier);
  }
  validate() {
    // these are the required validations
    this.validateSupplierAddress();
    this.validateSupplierCity();
    this.validateSupplierName();

    // if all required information is in the form we emit
    if (this.validSupplierAddress && this.validSupplierCity && this.validSupplierName) {
      this.emitSupplier(this.supplier);
    }
  }

  validateSupplierName(): void {
    if (this.supplier.name) {
      this.validSupplierName = true;
    } else {
      this.validSupplierName = false;
    }
  }
  validateSupplierAddress(): void {
    if (this.supplier.address) {
      this.validSupplierAddress = true;
    } else {
      this.validSupplierAddress = false;
    }
  }
  validateSupplierCity(): void {
    if (this.supplier.city) {
      this.validSupplierCity = true;
    } else {
      this.validSupplierCity = false;
    }
  }
}
