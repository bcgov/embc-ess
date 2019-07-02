# Ngrx Store (Client side data cache/sharing)

Due to time constraints the Ngrx store is established for a limited number of models and only used when significant benefit is found. 

**@ngrx/store** is a controlled state container designed to help write performant, consistent applications on top of Angular. Core tenets:

- State is a single immutable data structure
- Actions describe state changes
- Pure functions called reducers take the previous state and the next action to compute the new state
- State accessed with the Store, an observable of state and an observer of actions
  For more information see the [@ngrx/store git page](https://github.com/ngrx/store).

ngrx code is located in the **ClientApp/scr/app/store** directory

**Actions**: found in **ClientApp/scr/app/store/actions** 

An action has a type: This is a string constant that describes/identifies the action.

An action optionally has a payload which is passed through a constructor.
The payload is a parameter usually used to pass in the new value to update the state to. 

**Example 1**

```ts
import { Action } from '@ngrx/store';
import { Application } from '../../models/adoxio-application.model';

export const APPLICATION = 'APPLICATION';
export const SET_APPLICATION = 'SET_APPLICATION';

export class ApplicationsAction implements Action {
    readonly type = APPLICATION;
}

export class SetApplicationsAction implements Action {
    readonly type = SET_APPLICATION;

  constructor(public payload: Application[]) { }
}

export type Actions =
  ApplicationsAction
  | SetApplicationsAction;
```

**Models**: found in **ClientApp/scr/app/app-state/models**

In the models directory are files that describe the shape of the state object.
Notice that the AppState is comprised of smaller states. This is to allow 'sub-states' to manipulated indepentely.
It also allows components and service to only subscribe to a portion of the AppState.

**Example 2**

```ts
import { AdoxioLegalEntity } from '../../models/adoxio-legalentities.model';
import { DynamicsAccount } from '../../models/dynamics-account.model';
import { Application } from '../../models/adoxio-application.model';

export interface AppState {
    legalEntitiesState: LegalEntitiesState;
    applicationsState: ApplicationsState;
    currentAccountState: CurrentAccountState;
    currentApplicationState: CurrentApplicationState;
    currentLegalEntityState: CurrentLegalEntityState;
}

export interface LegalEntitiesState {
    legalEntities: AdoxioLegalEntity[];
}

export interface ApplicationsState {
    applications: Application[];
}

export interface CurrentAccountState {
    currentAccount: DynamicsAccount;
}
export interface CurrentApplicationState {
    currentApplication: Application;
}

export interface CurrentLegalEntityState {
    currentLegalEntity: AdoxioLegalEntity;
}
```

**Reducers**: found in **ClientApp/scr/app/app-state/reducers**
Each reducer file typically contains the action handler function (called a reducer) for a portion of state.

## Routing

Passing parameters on the URL(route)

In app-routing.module.ts

**Example 3**

```js
const routes: Routes = [
  {
    path: '',
    component: HomeComponent
  },
  {
    path: 'application-lite/:applicationId', //applicationId is a route parameter
    component: ApplicationComponent,
    canDeactivate: [CanDeactivateGuard]
  }
];
```

To read parameters from the route:

**Example 4**

```ts
import { Component, OnInit, OnDestroy, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-application',
  templateUrl: './application.component.html',
  styleUrls: ['./application.component.scss']
})
export class ApplicationComponent implements OnInit, OnDestroy {
    applicationId: string;

    constructor(private route: ActivatedRoute) {
      //Use this form if the route does not change (same route with different parameters) once the component is loaded
      this.applicationId = this.route.snapshot.paramMap.get('applicationId');
    }

    ngOnInit(): void {
      //Use this form if the route changes while the component is loaded
      this.route.ParamMap.subscribe(p => {
        this.applicationId = p.applicationId;
      });
    }

    //this get called because the route configuation has a CanDeactivate router guard
    canDeactivate(): Observable<boolean> | boolean {
      if (JSON.stringify(this.savedFormData) === JSON.stringify(this.form.value)) {
        return true; //return true if form value has not changed
      } else {
        return this.save(); //otherwise wait for the form to load the navigate away 
      }
    }

    save(showProgress: boolean = false): Subject<boolean> {
    const saveResult = new Subject<boolean>(); //used to exposed the result of the save operation
    const saveData = this.form.value;
    this.accountModel = this.toAccountModel(this.form.value);
    const sub = this.applicationDataService.updateAccount(this.accountModel).subscribe(
      res => {
        saveResult.next(true);
        this.savedFormData = saveData;
      },
      err => {
        saveResult.next(false); // this will stop router navigation
      });

    if (showProgress === true) {
      this.busy = sub;
    }
    return saveResult;
  }
}
```
