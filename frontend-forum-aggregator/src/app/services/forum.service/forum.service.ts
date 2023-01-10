import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ForumsResponse } from 'src/app/interfaces/forums-response';

@Injectable({
    providedIn: 'root'
})
export class ForumService 
{
    private searchForumUrl: string = "http://localhost:8000/forum/search/";
    private getForumUrl: string = "http://localhost:8000/forum/";

    constructor(private http: HttpClient) { }

    search(searchString: string): Observable<ForumsResponse[]> 
    {
        return this.http.get<ForumsResponse[]>(`${this.searchForumUrl}${searchString}`, { withCredentials: true });
    }

    getForumByName(name: string): Observable<any> 
    {
        return this.http.get<any>(`${this.getForumUrl}${name}`, { withCredentials: true });
    }

    getForum(forumId: string): Observable<any> 
    {
        return this.http.get<any>(`${this.getForumUrl}${forumId}`, { withCredentials: true });
    }

    getAllForums(): Observable<ForumsResponse[]>
    {
        return this.http.get<ForumsResponse[]>(`${this.getForumUrl}`, { withCredentials: true });
    }

    addForum(name: string, description: string)
    {
        return this.http.post(`${this.getForumUrl}`, {
            "name": name,
            "description": description,
            "moderators": [],
            "blacklist": []
        }, { withCredentials: true })
    }

    updateForum(forumId: string, name: string, description: string)
    {
        return this.http.patch(`${this.getForumUrl}`, {
            "forumId": forumId,
            "name": name,
            "description": description
        }, { withCredentials: true });
    }

    getUserAllForums(userId: string)
    {
        return this.http.get<ForumsResponse[]>(`${this.getForumUrl}user/${userId}`, { withCredentials: true });
    }

    deleteForum(forumId: string): Observable<any>
    {
        return this.http.delete<any>(`${this.getForumUrl}${forumId}`, { withCredentials: true });
    }
}
