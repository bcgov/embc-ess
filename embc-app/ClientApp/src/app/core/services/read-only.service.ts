import { Injectable } from '@angular/core';
import { CoreModule } from '../core.module';
import { Store } from '@ngrx/store';
import { AppState } from '../../store';
import { Config } from '../models';

@Injectable({
    providedIn: CoreModule
})
export class ReadOnlyService {
    private isReadOnly: boolean = false;

    constructor(private store: Store<AppState>,) { }

    public calculate() {
        this.store.select(s => s.lookups.config.config).subscribe((config: Config) => {
            this.isReadOnly = config.readOnlyMode;
        });
    }

    public getIsReadOnly() {
        return this.isReadOnly;
    }
}
