import { Routes } from '@angular/router';
import { RegisterComponent } from './pages/register/register.component';
import { LoginComponent } from './pages/login/login.component';
import { PatientListComponent } from './patients/patient-list/patient-list.component';
import { authGuard } from './core/guards/auth.guard';
import { DashboardComponent } from './pages/dashboard/dashboard.component';

export const routes: Routes = [
  {
    path: 'login',
    component: LoginComponent
  },

  {
  path: 'register',
  component: RegisterComponent
},

{
  path: 'dashboard',
  component: DashboardComponent,
  canActivate: [authGuard]
},

  {
    path: 'patients',
    component: PatientListComponent,
    canActivate: [authGuard]
  },
  {
    path: '',
    redirectTo: 'login',
    pathMatch: 'full'
  }
];