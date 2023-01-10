import { Component, EventEmitter, Output } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router } from '@angular/router';
import { LoginResponse } from '../interfaces/login-response';
import { AuthenticationService } from '../services/authentication.service/authentication.service';
import { ErrorService } from '../services/error.service/error.service';

@Component({
    selector: 'app-login',
    templateUrl: './login.component.html',
    styleUrls: ['./login.component.css']
})
export class LoginComponent 
{
    hide = true;

    email: string = "";
    password: string = "";

    constructor(
        private authService: AuthenticationService,
        private router: Router,
        private snackBar: MatSnackBar,
        private errorService: ErrorService
    ) { }

    submit(): void 
    {
        this.authService.login(this.email, this.password).subscribe({
            next: (v) => {
                localStorage.setItem("logged_in", "true");
                localStorage.setItem("current_user", JSON.stringify({
                    "userId": v.id,
                    "userName": v.name
                }));
                this.router.navigateByUrl('/', { skipLocationChange: true }).then(() => this.router.navigate(["posts"]));
            },
            error: error => this.errorService.dealWith(error)
        });
    }
}
