import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { first } from 'rxjs/operators';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})

export class LoginComponent implements OnInit {
  
  public loginForm = new FormGroup({
    username: new FormControl('', Validators.required),
    password: new FormControl('', Validators.required),
  })
  
  loading = false;
  submitted = false;
  returnUrl!: string;
  error = '';

  constructor(
    private formBuilder: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private authService: AuthService
  ){
    if (this.authService.userValue) {
      this.router.navigate(['/']);
    }
  }

  ngOnInit(): void {
    this.loginForm = this.formBuilder.group({
      username: ['', Validators.required],
      password: ['', Validators.required]
    });
    this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/contacts';
  }

  onSubmit() {

    this.submitted = true;
    if (this.loginForm.invalid) {
        return;
    }
    this.loading = true;
    this.authService.login({
      login: this.loginForm.controls.username.value!,
      password: this.loginForm.controls.password.value!
    })
    // .pipe(first())
    .subscribe(data => {
      this.router.navigate([this.returnUrl]);
    }, 
    error => {
      this.error = error;
      this.loading = false;
    });
  }

  register(){ 

    this.authService.register({
      login: this.loginForm.controls.username.value!,
      password: this.loginForm.controls.password.value!
    })
    .subscribe(data => {
      this.router.navigate([this.returnUrl]);
    },
    error => {
        this.error = error;
        this.loading = false;
    });
  }
}