import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
    providedIn: 'root'
})
export class UserService {
    private userUrl: string = "http://localhost:8000/user"

    constructor(private http: HttpClient) { }

    getUser(userId: string): Observable<any>
    {
        return this.http.get(`${this.userUrl}/${userId}`, { withCredentials: true });
    }
}
