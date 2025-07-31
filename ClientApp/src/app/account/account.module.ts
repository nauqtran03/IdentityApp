import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { AccountRoutingModule } from './account-routing.module';

@NgModule({
  declarations: [],
  imports: [
    CommonModule,AccountRoutingModule,LoginComponent,RegisterComponent
  ]
})
export class AccountModule { }
