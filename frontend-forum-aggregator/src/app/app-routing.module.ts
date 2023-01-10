import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ForumFormComponent } from './forum-form/forum-form.component';
import { ForumsComponent } from './forums/forums.component';
import { LoginComponent } from './login/login.component';
import { PostFormComponent } from './post-form/post-form.component';
import { PostsComponent } from './posts/posts.component';
import { RegistrationComponent } from './registration/registration.component';

export const routes: Routes = [
  { path: '', redirectTo: '/posts', pathMatch: 'full' },
  { path: "posts", component: PostsComponent },
  { path: "login", component: LoginComponent },
  { path: "register", component: RegistrationComponent },
  { path: "post/create", component: PostFormComponent },
  { path: "post/update/:id", component: PostFormComponent },
  { path: "user/:id/posts", component: PostsComponent },
  { path: "user/:id/forums", component: ForumsComponent },
  { path: "forums", component: ForumsComponent },
  { path: "forum/create", component: ForumFormComponent },
  { path: "forum/update/:id", component: ForumFormComponent },
  { path: "forum/:forumId/posts", component: PostsComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
