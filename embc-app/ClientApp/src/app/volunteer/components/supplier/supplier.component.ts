import { Component, Input, Output, EventEmitter, OnInit } from '@angular/core';
import { Supplier } from 'src/app/core/models';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';

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

  // The model for the form data collected
  form: FormGroup;

  validSupplierName = true;
  validSupplierAddress = true;
  validSupplierCity = true;

  readonly phoneMask = [/\d/, /\d/, /\d/, '-', /\d/, /\d/, /\d/, '-', /\d/, /\d/, /\d/, /\d/]; // 999-999-9999

  constructor(private fb: FormBuilder) { }

  ngOnInit() {
    this.initForm();
    this.displaySupplier(this.supplier);
  }

  initForm(): void {
    this.form = this.fb.group({
      name: ['', Validators.required],
      address: ['', Validators.required],
      city: ['', Validators.required],
      postalCode: '',
      telephone: '',
      fax: ''
    });
  }

  displaySupplier(supplier: Supplier) {
    if (supplier) {
      this.form.reset();
      this.form.patchValue({
        name: supplier.name,
        address: supplier.address,
        city: supplier.city,
        postalCode: supplier.postalCode,
        telephone: supplier.telephone,
        fax: supplier.fax,
      });
    }
  }

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
