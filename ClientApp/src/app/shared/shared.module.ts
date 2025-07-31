import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { NotFoundComponent } from './components/errors/not-found/not-found.component';
import { ValidationMessagesComponent } from './components/errors/validation-messages/validation-messages.component';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { ModalModule} from 'ngx-bootstrap/modal'


@NgModule({
  declarations: [
    
  ],
  imports: [
    CommonModule,RouterModule,NotFoundComponent,
    ValidationMessagesComponent,HttpClientModule,ModalModule.forRoot()
  ],
  exports: [
    RouterModule,ReactiveFormsModule,HttpClientModule,ValidationMessagesComponent,ModalModule
  ]
})
export class SharedModule { }
