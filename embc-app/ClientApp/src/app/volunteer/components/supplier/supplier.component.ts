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
  private initForm(): void {
    this.form = this.fb.group({
      name: ['', Validators.required],
      address: ['', Validators.required],
      city: ['', Validators.required],
      province: '',
      postalCode: '',
      telephone: '',
      fax: ''
    });
  }

  // validate the whole form as we capture data
  private handleFormChange(): void {
    this.form.valueChanges.subscribe(() => this.validate());
  }

  private displaySupplier(supplier: Supplier) {
    if (supplier) {
      this.form.reset();
      this.form.patchValue({
        name: supplier.name,
        address: supplier.address,
        city: supplier.city,
        province: supplier.province,
        postalCode: supplier.postalCode,
        telephone: supplier.telephone,
        fax: supplier.fax,
      });
    }
  }

  // if all required information is in the form we emit
  private validate() {
    if (this.form.valid) {
      const supplier: Supplier = { ...this.form.value };
      // if telephone or fax are blank then delete them from object
      if (supplier.telephone === '___-___-____') { delete supplier.telephone; }
      if (supplier.fax === '___-___-____') { delete supplier.fax; }
      this.supplierChange.emit(supplier);
    }
  }

}
