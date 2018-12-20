import { Injectable } from '@angular/core';
import { Todo } from '../_models';
import { environment } from '../../environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, of } from 'rxjs';

const API_URL = environment.apiUrl;

@Injectable({ providedIn: 'root' })
export class TodoDataService {

    todos: Todo[] = [];

    constructor(
        private http: HttpClient
    ) {
    }
/*
    // Simulate POST /todos
    addTodo(todo: Todo) {
        return this.api.createTodo(todo);
    }

    // Simulate DELETE /todos/:id
    deleteTodoById(todoId: string) {
        return this.api.deleteTodoById(todoId);
    }

    // Simulate PUT /todos/:id
    updateTodo(todo: Todo) {
        return this.api.updateTodo(todo);
    }
    */
    // GET /todos
    getAllTodos(): Observable<Todo[]> {
        //return this.api.getAllTodos();
        let user = JSON.parse(localStorage.getItem('currentUser'));

        const headers = new HttpHeaders({
            'Content-Type': 'application/json',
            'Authorization': user.token
        })

        return this.http.get<Todo[]>(`${API_URL}/api/todo`, { headers: headers });

    }

    /*
    // Simulate GET /todos/:id
    getTodoById(todoId: string) {
        return this.api.getTodoById(todoId);
    }

    // Toggle complete
    toggleTodoComplete(todo: Todo) {
        todo.complete = !todo.complete;
        return this.api.updateTodo(todo);
    }
    */

}

/*

import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { ToDo } from '../_models/todo';

@Injectable({ providedIn: 'root' })

export class TodoDataService {

  todos: ToDo[] = [];

    constructor(private http: HttpClient) { }

    addToDo(todo: ToDo) {

        return this.http.post<any>('http://localhost:8840/api/todo', { todo })
            .pipe(map(todoItem => {
                // login successful if there's a jwt token in the response
                if (todoItem && todoItem.id) {
                    this.todos.push(todo);                }

                return this;
            }));
    }

    deleteTodoById(id: string) {
        return this.http.delete<any>('http://localhost:8840/api/todo' + id )
            .pipe(map(response => {
                // login successful if there's a jwt token in the response
                if (response.status == 204) {
                    this.todos = this.todos
                        .filter(todo => todo.id !== id);
                    return this;
                    }
                }));
    }

    getAllTodos() {

        let user = JSON.parse(localStorage.getItem('currentUser'));

        const headers = new HttpHeaders({
            'Content-Type': 'application/json',
            'Authorization': user.token
        })

        return this.http.get<any>('http://localhost:8840/api/todo', { headers: headers })
            .pipe(map(todoItem => {
                // login successful if there's a jwt token in the response
                if (todoItem ) {
                    this.todos = todoItem;
                }

                return this;
            }));
    }
}
*/