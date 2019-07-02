# EMBC App

This project was generated with [Angular CLI](https://github.com/angular/angular-cli) version 7.3.0.

- Npm version - 6.4.1
- Node version - 10.15.1
- Angular version - 7.2.0

## Installation prerequisite

Run `npm install` in `ClientApp` folder. To reinstall, delete the `node_modules` directory and re-run `npm install`.

## Development server

<!-- Run `ng serve` for a dev server. Navigate to `http://localhost:4200/`. The app will automatically reload if you change any of the source files. -->

### Basic Setup Procedure

A basic setup of the development environment follows this procedure:

1. Install Docker Desktop, Node and Angular-cli. (global installs)

2. Install npm packages locally using `npm install`. This is optional but prevents your IDE from throwing up errors and it checks if all packages are available. (For IDE hints.)

3. Run Docker Desktop

4. Run `docker-rebuild.sh` to create a docker container with all of the required packages.

5. Make an `.env` file. The `API_URL` environment variable must point to a running version of the API. That API must be running in a development environment. You have to get the developer secrets for the `SM_TOKEN` from an official development team.

6. Run `start.sh` and leave the bash window open.

If you are asked for volumes permissions you must agree otherwise docker can't share the folder on your local hard-drive with the docker container. You should be able to make changes in the components and see the docker container reload your changes. The local version is now running at `http://localhost:4200` in a web browser.

### Environment Proxy and Tokens

It is mandatory that the foront-end application have an API to consume. The front-end respects site-minder so we need a "back door" for developers to get past. The back-door is programmed back-end only and is enabled if the project is running in a specifically designated development environment. To access the development API you need to set the proxy and token in the `.env` file in a  way similar to seen in `.env-example`.

There are three different `SM_TOKEN`s that are needed to use the front-end locally. The tokens are paired to front-end roles. To see a new view the `.env` file should be modified to comment all users except for the one being developed for. A developer cannot achieve a "logged out" state if these are enabled but can manually route to public areas that are not guarded.
   
   - **volunteer** - A standard ESS user is known as a volunteer. This role is for tasks done by the volunteers in an evacuation centre. This user accesses a production deployed environment using their BCeID.
   - **local authority** - A privileged user for an organization that can create "volunteers" and manage a team. This user accesses a production deployed environment using their BCeID. This user works for a local organization such as a fire-department.
   - **provincial admin** - An administrator user for the provincial government. This user can manage other users and organizations. This user accesses a production deployed environment using their IDIR.

### Docker

Local development is done inside of a docker container. The docker container shares space on the development machine using a docker `volume`. The docker container then runs the front-end using the packages as listed in `package.json` instead of using the version of node and angular cli found installed globally on the development machine. 

When adding or removing packages to the docker container it adds the additional step of rebuilding the docker container. Fortunately, the repository comes with convenience scripts for most of the tasks that can be run in unix-like shells.

> `build.sh` Confirms that a build will succed after changes are made in the project. Because the docker container has the same resources as the container for the production build there should be no difference between the front-end code running locally and the front-end code running on OpenShift.

> `docker-clean.sh` Removes old and dead containers.

> `docker-shell.sh` Opens a shell into the embc-app-frontend. This allows you to inspect installed dependencies in the npm folder or make changes to packages without rebuilding the container.

> `docker-rebuild.sh` Rebuild the docker container after making a change to the node packages.

> `start.sh` Start the docker container and development server. This angular development server will restart itself when it detects typescript changes.

> `stop.sh` If a window crashed or closed and left the docker container running it can generate errors about port 4200 being in-use it can be helpful to manually stop the docker container and ensure that the port is available.

## Code scaffolding

Run `ng generate component component-name` to generate a new component. You can also use `ng generate directive|pipe|service|class|guard|interface|enum|module`.

## Build

Run `build.sh` to build the project inside of the docker container.

<!-- ## Running unit tests

Run `ng test` to execute the unit tests via [Karma](https://karma-runner.github.io).

## Running end-to-end tests

Run `ng e2e` to execute the end-to-end tests via [Protractor](http://www.protractortest.org/). -->

## Ngrx Store (Client side data cache/sharing)

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

## Reactive forms

When creating angular forms, it is important to have the 'shape' of the form-group match that of the data-model
that the form deals with. This makes it easier to get and set the form values.

See the [angular reactive forms page](https://angular.io/guide/reactive-forms#creating-nested-form-groups) for reference.

## Mobile styling

For information on the Bootstrap grid system [see](https://getbootstrap.com/docs/4.0/layout/grid/).

## Debugging in Chrome

For information about debugging in chrome vist the [chrome-devtools documentation](https://developers.google.com/web/tools/chrome-devtools/javascript/).
