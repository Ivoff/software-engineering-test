import { Component, Input } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ActivatedRoute, Router } from '@angular/router';
import { Post } from '../interfaces/post';
import { ErrorService } from '../services/error.service/error.service';
import { ForumService } from '../services/forum.service/forum.service';
import { PostService } from '../services/post.service';
import { UserService } from '../services/user.service/user.service';


@Component({
	selector: 'app-posts',
	templateUrl: './posts.component.html',
	styleUrls: ['./posts.component.css']
})
export class PostsComponent {
	posts: Post[] = [];
	currentUserId?: string;
	currentQueryUserId?: string;
	currentUserName: string = "";
	sameUser: boolean = false;	
	currentForumId?: string;
	currentFoumName?: string;
	headerName: string = "Posts";
	search: string = "";

	constructor(
		private postService: PostService,
		private router: Router,
        private route: ActivatedRoute,
		private snackBar: MatSnackBar,
		private errorService: ErrorService,
		private forumService: ForumService,
		private userService: UserService
	) { }

	initGetPosts(): void {
		this.postService.getPosts().subscribe({
			next: (value) => this.posts = value,
			error: (error) => this.errorService.dealWith(error)
		});
	}

	initGetUserPosts(): void
	{
		this.userService.getUser(this.currentQueryUserId!).subscribe({
			next: (value) => {
				this.currentUserName = value.name;
				this.headerName = `${this.currentUserName}'s Posts`;
			},
			error: error => this.errorService.dealWith(error)
		});

		this.postService.getUserPosts(this.currentQueryUserId!).subscribe({
			next: value => this.posts = value,
			error: error => this.errorService.dealWith(error)
		});
	}

	initGetForumPosts(): void
	{
		this.forumService.getForum(this.currentForumId!).subscribe({
			next: (value) => {
				this.currentFoumName = value.name;
				this.headerName = `${this.currentFoumName}'s Posts`;
			},
			error: (error) => this.errorService.dealWith(error)
		});

		this.postService.getForumPosts(this.currentForumId!).subscribe({
			next: (value) => {
				this.posts = value;
			},
			error: (error) => this.errorService.dealWith(error)
		});
	}

	deletePost(postId: string)
	{
		this.postService.deleletePost(postId).subscribe({
            next: (value) => {
				this.initGetUserPosts()
            },
			error: error => this.errorService.dealWith(error)
        });
	}

	openSnackBar(message: string, action: string)
	{
		this.snackBar.open(message, action, {
			duration: 5000
		});
	}

	ngOnInit() {
		this.currentQueryUserId = this.route.snapshot.params["id"];
		this.currentForumId = this.route.snapshot.params["forumId"];

		if (localStorage.getItem("logged_in") == "true") {
			let currentUser = JSON.parse(localStorage.getItem("current_user")!);
			this.currentUserId = currentUser.userId;
            this.currentUserName = currentUser.userName;
		}

		if (this.currentQueryUserId != null && this.currentQueryUserId != "")
		{
			this.initGetUserPosts();
			this.sameUser =  this.currentUserId == this.currentQueryUserId;
		}			
		else if (this.currentForumId != null && this.currentForumId != "")
			this.initGetForumPosts();
		else
			this.initGetPosts();

	}
}
