import { Component, Input, Output, EventEmitter, OnInit, OnChanges, SimpleChanges } from '@angular/core';
import { Supplier } from 'src/app/core/models';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';

// poor man's uuid
let identifier = 0;

@Component({
  selector: 'app-supplier',
  templateUrl: './supplier.component.html',
  styleUrls: ['./supplier.component.scss']
})
export class SupplierComponent implements OnInit, OnChanges {
  @Input() showErrorsWhen = true;
  @Input() readOnly = false;
  @Input() supplier: Supplier;

  @Output() formReady = new EventEmitter<FormGroup>();
  @Output() supplierChange = new EventEmitter<Supplier>();

  // The model for the form data collected
  supplierForm = this.fb.group({
    name: ['', Validators.required],
    address: ['', Validators.required],
    city: ['', Validators.required],
    province: '',
    postalCode: '',
    telephone: '',
    fax: ''
  });

  uuid = `${identifier++}`;

  readonly phoneMask = [/\d/, /\d/, /\d/, '-', /\d/, /\d/, /\d/, '-', /\d/, /\d/, /\d/, /\d/]; // 999-999-9999

  constructor(private fb: FormBuilder) { }

  ngOnInit() {
    // Emit the form group to the father to do whatever it wishes
    this.formReady.emit(this.supplierForm);
  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes.supplier && this.supplier && !this.readOnly) {
      this.supplierForm.patchValue({
        name: this.supplier.name,
        address: this.supplier.address,
        city: this.supplier.city,
        province: this.supplier.province,
        postalCode: this.supplier.postalCode,
        telephone: this.supplier.telephone,
        fax: this.supplier.fax,
      });
    }
  }
}
