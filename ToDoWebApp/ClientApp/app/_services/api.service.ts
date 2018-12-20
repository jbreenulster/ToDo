import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
// import { Http, Response } from '@angular/http';
import { Todo } from '../_models';
import { map } from 'rxjs/operators';
import { HttpClient, HttpHeaders } from '@angular/common/http';


@Injectable()
export class ApiService {
    todos: Todo[] = [];
    constructor(
        private http: HttpClient
    ) {
    }

    public getAllTodos() {
        let user = JSON.parse(localStorage.getItem('currentUser'));

        const headers = new HttpHeaders({
            'Content-Type': 'application/json',
            'Authorization': user.token
        })

        const API_URL = environment.apiUrl;

        return this.http.get<any>(API_URL, { headers: headers })
            .pipe(map(todoItem => {
                if (todoItem) {
                    this.todos = todoItem;
                }

                return this;
            }));

    }
/*
    public createTodo(todo: Todo){
        return this.http
            .post(API_URL + '/todos', todo)
            .pipe(map(response => {
                return new Todo(response.json());
            }),
            catchError(this.handleError));
    }

    public getTodoById(todoId: string) {
        return this.http
            .get(API_URL + '/todos/' + todoId)
            .pipe(map(response => {
                return new Todo(response.json());
            }),
            catchError(this.handleError));
    }

    public updateTodo(todo: Todo) {
        return this.http
            .put(API_URL + '/todos/' + todo.id, todo)
            .pipe(map(response => {
                return new Todo(response.json());
            }),
                catchError(this.handleError));
    }

    public deleteTodoById(todoId: string) {
        return this.http
            .delete(API_URL + '/todos/' + todoId)
            .pipe(map(response => null),
            catchError(this.handleError));
    }

    private handleError(error: Response | any) {
        console.error('ApiService::handleError', error);
        return error;
    }
    */
}