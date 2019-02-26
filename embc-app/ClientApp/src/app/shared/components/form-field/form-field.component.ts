import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-form-field',
  templateUrl: './form-field.component.html',
  styleUrls: ['./form-field.component.scss']
})
export class FormFieldComponent implements OnInit {
  @Input() required = false;
  @Input() showChevrons = true;
  @Input() valid = true;
  @Input() errorMessage: string;

  constructor() { }

  ngOnInit() {
  }

}
