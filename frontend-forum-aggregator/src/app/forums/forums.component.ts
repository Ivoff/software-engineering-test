import { Component } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ActivatedRoute } from '@angular/router';
import { ForumsResponse } from '../interfaces/forums-response';
import { ErrorService } from '../services/error.service/error.service';
import { ForumService } from '../services/forum.service/forum.service';

@Component({
  selector: 'app-forums',
  templateUrl: './forums.component.html',
  styleUrls: ['./forums.component.css']
})
export class ForumsComponent 
{
    forums:ForumsResponse[] = [];
    currentUserId?: string;
	currentQueryUserId?: string;
    currentUserName?: string;
	sameUser: boolean = false;

    constructor(
        private forumService: ForumService,
        private snackBar: MatSnackBar,
        private route: ActivatedRoute,
        private errorService: ErrorService
    ){}

    getForums()
    {
        this.forumService.getAllForums().subscribe({
            next: (value: ForumsResponse[]) => {
                this.forums = value;
            },
            error: error => this.errorService.dealWith(error)
        })
    }

    getForumsFromUser()
    {
        this.forumService.getUserAllForums(this.currentQueryUserId!).subscribe({
            next: (value: ForumsResponse[]) => {
                this.forums = value;
            },
            error: error => this.errorService.dealWith(error)
        });
    }

    deleteForum(forumId: string)
    {
        this.forumService.deleteForum(forumId).subscribe({
            next: res => {
                this.openSnackBar("Forum successfully deleted.", "Dismiss");
                this.getForumsFromUser();
            },
            error: error => this.errorService.dealWith(error)
        })
    }

    openSnackBar(message: string, action: string)
    {
        this.snackBar.open(message, action);
    }

    ngOnInit()
    {
        this.currentQueryUserId = this.route.snapshot.params["id"];

		if (localStorage.getItem("logged_in") == "true") 
        {
            let currentUser = JSON.parse(localStorage.getItem("current_user")!);
			this.currentUserId = currentUser.userId;
            this.currentUserName = currentUser.userName;
		}

        if (this.currentQueryUserId != null && this.currentQueryUserId != "null")
        {
            this.sameUser =  this.currentUserId == this.currentQueryUserId;
            this.getForumsFromUser();
        }
        else
        {
            this.getForums();
        }
    }
}
