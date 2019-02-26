import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { HomeComponent } from './home/home.component';
import {
  SelfRegistrationComponent,
  SelfRegistrationOneComponent,
  SelfRegistrationTwoComponent,
  SelfRegistrationThreeComponent,
  SelfRegistrationFourComponent,
} from './self-registration';

const routes: Routes = [
  {
    path: '',
    component: HomeComponent
  },
  {
    path: 'self-registration',
    component: SelfRegistrationComponent,
    children: [
      {
        path: '',
        redirectTo: 'step-1',
        pathMatch: 'full'
      },
      {
        path: 'step-1',
        component: SelfRegistrationOneComponent
      },
      {
        path: 'step-2',
        component: SelfRegistrationTwoComponent
      },
      {
        path: 'step-3',
        component: SelfRegistrationThreeComponent
      },
      {
        path: 'step-4',
        component: SelfRegistrationFourComponent
      },
    ]
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes, { scrollPositionRestoration: 'top' })],
  exports: [RouterModule]
})
export class AppRoutingModule { }
