import { Component, Input } from '@angular/core';
import { Router } from '@angular/router';
import { AuthenticationService } from '../services/authentication.service/authentication.service';


@Component({
    selector: 'app-toolbar',
    templateUrl: './toolbar.component.html',
    styleUrls: ['./toolbar.component.css']
})
export class ToolbarComponent {
    @Input() title?: string;
    value: string = "";
    loggedIn?: boolean;
    userName?: string;
    userId?: string;

    constructor(
        public authService: AuthenticationService,
        private router: Router
    ) { }

    updateAppUserContext(): void 
    {
        this.loggedIn = localStorage.getItem("logged_in") === "true";
        if (this.loggedIn) {
            let currentUser = JSON.parse((localStorage.getItem("current_user") as string));
            this.userName = currentUser.userName;
            this.userId = currentUser.userId;
        }
    }

    onRouterActive(event: any) 
    {
        console.log("teste");
        this.updateAppUserContext();
    }
}
