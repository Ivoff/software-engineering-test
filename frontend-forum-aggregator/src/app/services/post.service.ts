import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Post } from '../interfaces/post';
import { Observable } from 'rxjs';

@Injectable({
    providedIn: 'root'
})
export class PostService {

    private postUrl: string = "http://localhost:8000/post";
    private userPostsUrl: string = "http://localhost:8000/post/user";
    private forumPostsUrl: string = "http://localhost:8000/post/forum";

    constructor(private http: HttpClient) { }

    getPosts() {
        return this.http.get<Post[]>(this.postUrl)
    }

    getPost(id: string): Observable<Response>
    {
        return this.http.get<Response>(`${this.postUrl}/${id}`, { withCredentials: true });
    }

    addPost(forumId: string, title: string, content: string): Observable<Response> 
    {
        return this.http.post<Response>(this.postUrl, {
            "forumId": forumId,
            "title": title,
            "content": content
        }, { withCredentials: true });
    }

    updatePost(postId: string, title: string, content: string): Observable<Response> 
    {
        return this.http.patch<Response>(this.postUrl, {
            "postId": postId,
            "title": title,
            "content": content
        }, { withCredentials: true });
    }

    getUserPosts(userId: string)
    {
        return this.http.get<Post[]>(`${this.userPostsUrl}/${userId}`, { withCredentials: true });
    }

    getForumPosts(forumId: string)
    {
        return this.http.get<Post[]>(`${this.forumPostsUrl}/${forumId}`, { withCredentials: true });
    }

    deleletePost(postId: string): Observable<Response>
    {
        return this.http.delete<Response>(`${this.postUrl}/${postId}`, { withCredentials: true })
    }

}
