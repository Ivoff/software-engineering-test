import { Component } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ActivatedRoute, Router } from '@angular/router';
import { right } from '@popperjs/core';
import { Observable } from 'rxjs';
import { ErrorService } from '../services/error.service/error.service';
import { ForumService } from '../services/forum.service/forum.service';
import { Location } from '@angular/common';

@Component({
  selector: 'app-forum-form',
  templateUrl: './forum-form.component.html',
  styleUrls: ['./forum-form.component.css']
})
export class ForumFormComponent 
{
    name: string = "";
    description: string = "";

    update: boolean = false;
    forumId: string = "";

    constructor(
        private forumService: ForumService,
        private snackBar: MatSnackBar,
        private router: Router,
        private route: ActivatedRoute,
        private errorService: ErrorService,
        public location: Location
    )
    {
        this.update = this.route.snapshot.params["id"] != null && this.route.snapshot.params["id"] != "";
        this.forumId = this.route.snapshot.params["id"];
        if (this.update)
            this.getForumUpdate(this.forumId);
    }
    
    submit()
    {
        if (this.update)
            this.updateForum();
        else
            this.addForum();
    }

    updateForum()
    {
        this.forumService.updateForum(this.forumId, this.name, this.description).subscribe({
            next: () => {
                this.openSnackBar("Forum updated successfully.", "Dismiss");
                this.router.navigate(["/forums"]);
            },
            error: (error) => this.errorService.dealWith(error)
        })
    }

    addForum()
    {
        this.forumService.addForum(this.name, this.description).subscribe({
            next: () => {
                this.openSnackBar("Forum created successfully.", "Dismiss");                
                this.router.navigate(["/forums"]);
            },
            error: (error) => this.errorService.dealWith(error)
        })
    }

    getForumUpdate(forumId: string)
    {
        this.forumService.getForum(forumId).subscribe({
            next: (value: any) => {
                this.name = value.name,
                this.description = value.description;
            },
            error: (error) => this.errorService.dealWith(error)
        })
    }

    openSnackBar(message: string, action: string)
    {
        this.snackBar.open(message, action, {
			duration: 5000
		});
    }
}
