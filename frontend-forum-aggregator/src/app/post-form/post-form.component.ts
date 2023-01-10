import { Component, Input } from '@angular/core';
import { FormControl } from '@angular/forms';
import { ForumService } from '../services/forum.service/forum.service';
import { Observable } from 'rxjs';
import { map, startWith } from 'rxjs/operators';
import { ForumsResponse } from '../interfaces/forums-response';
import { PostService } from '../services/post.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router } from '@angular/router';
import { ActivatedRoute } from '@angular/router';
import { PostResponse } from '../interfaces/post-response';
import { ErrorService } from '../services/error.service/error.service';
import { HttpErrorResponse } from '@angular/common/http';
import { Location } from '@angular/common';

@Component({
    selector: 'app-post-form',
    templateUrl: './post-form.component.html',
    styleUrls: ['./post-form.component.css']
})
export class PostFormComponent 
{
    forumOptions!: Observable<ForumsResponse[]>;
    forumField = new FormControl('');
    update: boolean;
    postId?: string;
    forumId?: string;
    title?: string;
    content?: string;
    forumName?: string;

    constructor(
        private forumService: ForumService,
        private postService: PostService,
        private snackBar: MatSnackBar,
        private router: Router,
        private route: ActivatedRoute,
        private errorService: ErrorService,
        public location: Location
    ) 
    {
        this.update = this.route.snapshot.params["id"] != null && this.route.snapshot.params["id"] != "";
        this.postId = this.route.snapshot.params["id"];
        if (this.update)
        this.getPostUpdate(this.postId!);
    }

    // getForums(): Observable<ForumsResponse[]> {
    //   return this.forumOptions.pipe(map((forum) => Object.values(forum)));
    // }

    submit() 
    {
        if(this.update)
            this.updatePost();
        else
            this.addPost();
    }

    updatePost()
    {
        this.postService.updatePost(this.postId!, this.title!, this.content!).subscribe({
            next: () => {
                this.openSnackBar("Post successfully updated.", "Dismiss");
                this.location.back();
                // this.router.navigate(["/posts"])
            },
            error: error => this.errorService.dealWith(error)
        });   
    }

    addPost()
    {
        if (this.forumId == "" || this.forumId == null)
        {
            this.openSnackBar("Forum selected not recognized.", "Dismiss");
            return;
        }

        this.postService.addPost(this.forumId!, this.title!, this.content!).subscribe({
            next: () => {
                this.openSnackBar("Post successfully created.", "Dismiss");
                this.location.back();
                // this.router.navigate(["/posts"])
            },
            error: error => this.errorService.dealWith(error)
        });
    }

    openSnackBar(message: string, action: string)
    {
        this.snackBar.open(message, action,);
    }

    getPostUpdate(postId: string )
    {
        this.postService.getPost(postId).subscribe({
            next: (value: any) => {
                this.forumId = value.forumId;
                this.title = value.title;
                this.content = value.content;
                this.forumService.getForum(this.forumId!).subscribe({
                    next: (otherValue: any) => {
                        this.forumName = otherValue.name;
                        this.forumField.setValue(otherValue.name);
                    },
                    error: error => this.errorService.dealWith(error)
                })
            },
            error: error => this.errorService.dealWith(error)
        });
    }

    forumNameAutoCompleteFetcher()
    {
        this.forumField.valueChanges.subscribe((value) => {
            if (value != '' && this.update == false)
            {
                this.forumOptions = this.forumService.search(value!);
                this.forumName = this.forumField.value as string;
                if (this.forumName == "" || this.forumName == null)
                    return;

                this.forumService.getForumByName(this.forumName!).subscribe({
                    next: (value) => {
                        this.forumId = value.id;
                    },
                    error: (error) => {
                        console.error(`${error.status}: ${error.error}`);
                    }
                });
            }
        })
    }

    ngOnInit() {
        if (this.update == false)
            this.forumNameAutoCompleteFetcher();
    }
}
