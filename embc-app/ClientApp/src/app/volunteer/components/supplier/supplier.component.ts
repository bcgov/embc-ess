import { Component, Input, Output, EventEmitter, OnInit } from '@angular/core';
import { Supplier } from 'src/app/core/models';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';

// poor man's uuid
let identifier = 0;

@Component({
  selector: 'app-supplier',
  templateUrl: './supplier.component.html',
  styleUrls: ['./supplier.component.scss']
})
export class SupplierComponent implements OnInit {

  @Input() showErrorsWhen = true;
  @Input() readOnly = false;
  @Input() supplier: Supplier;
  @Output() formReady = new EventEmitter<FormGroup>();
  @Output() supplierChange = new EventEmitter<Supplier>();

  // The model for the form data collected
  form: FormGroup;

  uuid = `${identifier++}`;

  readonly phoneMask = [/\d/, /\d/, /\d/, '-', /\d/, /\d/, /\d/, '-', /\d/, /\d/, /\d/, /\d/]; // 999-999-9999

  constructor(private fb: FormBuilder) { }

  ngOnInit() {
    this.initForm();
    this.handleFormChange();
    this.displaySupplier(this.supplier);
  }

  // Create form controls
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

  // validate the whole form as we capture data
  handleFormChange(): void {
    this.form.valueChanges.subscribe(() => this.validate());
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

  // if all required information is in the form we emit
  validate() {
    if (this.form.valid) {
      this.emitSupplier({ ...this.form.value });
    }
  }
}
