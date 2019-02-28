import { NgModule, Optional, SkipSelf } from '@angular/core';
import { CommonModule } from '@angular/common';

import { throwIfAlreadyLoaded } from './guards/module-import.guard';
import { ControlledListService } from './services/controlled-list.service';

@NgModule({
  declarations: [],
  imports: [
    CommonModule
  ],
  providers: [
    ControlledListService,
  ]
})
export class CoreModule {
  constructor(@Optional() @SkipSelf() parentModule: CoreModule) {
    throwIfAlreadyLoaded(parentModule, 'CoreModule');
  }
}
