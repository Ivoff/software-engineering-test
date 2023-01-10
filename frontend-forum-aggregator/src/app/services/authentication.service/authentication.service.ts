import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { catchError, Observable, throwError } from 'rxjs';
import { Router } from '@angular/router';

@Injectable({
    providedIn: 'root'
})
export class AuthenticationService {
    private loginUrl: string = "http://localhost:8000/auth/login";
    private registerUrl: string = "http://localhost:8000/auth/register";
    private logoutUrl: string = "http://localhost:8000/auth/logout";

    constructor(private http: HttpClient, private router: Router) { }

    login(email: string, password: string): Observable<any> 
    {
        return this.http.post(
            this.loginUrl,
            {
                "email": email,
                "password": password
            }, { withCredentials: true }
        )
    }

    registrate(email: string, name: string, password: string): Observable<any> 
    {
        return this.http.post(
            this.registerUrl,
            {
                "email": email,
                "name": name,
                "password": password
            }, { withCredentials: true }
        )
    }

    logout(): void 
    {
        this.http.get(this.logoutUrl);
        localStorage.clear();
        this.router.navigate(["posts"]);
    }
}
