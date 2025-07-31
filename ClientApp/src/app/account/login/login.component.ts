import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { SharedService } from '../../shared/shared.service';
import { AccountService } from '../account.service';
import { SharedModule } from '../../shared/shared.module';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';


@Component({
  selector: 'app-login',
  standalone: true,
  imports: [SharedModule,CommonModule,HttpClientModule,ReactiveFormsModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent implements OnInit {

  loginForm: FormGroup = new FormGroup({});
    submitted = false;
    errorMessage: string[] = [];
  constructor(
    private accountService: AccountService,
    private formBuilder: FormBuilder,
    private router:Router) {
  }

  ngOnInit(): void {
    this.initializeForm();
  }

  initializeForm() {
      this.loginForm = this.formBuilder.group({
        userName: ['', Validators.required],
        password: ['', Validators.required]
      });
    }
  login() {
    this.submitted = true;
    this.errorMessage = [];
    //if(this.loginForm.valid){
      this.accountService.login(this.loginForm.value).subscribe({
        next: (response: any) => {

        },
        error: error =>{
          if(error.error.errors){
            this.errorMessage=error.error.errors;
          }else{
            this.errorMessage.push(error.error);
          }
        }
      });
    //}
  }
}
