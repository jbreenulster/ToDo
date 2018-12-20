import { Component, OnInit } from '@angular/core';
// import { first } from 'rxjs/operators';
import { User } from '../_models';
import { UserService } from '../_services';
import { Todo } from '../_models';
import { TodoDataService } from '../_services';

@Component({ selector: 'app', templateUrl: 'home.component.html', providers: [TodoDataService] })
export class HomeComponent implements OnInit {
    users: User[] = [];
    user: User;
    todos: any = [];

    constructor(private userService: UserService,
        private todoDataService: TodoDataService) { }

    ngOnInit() {
        
        let user = JSON.parse(localStorage.getItem('currentUser'));
        this.users.push(user);
        this.todoDataService
            .getAllTodos()
            .subscribe((data: {}) => {
                this.todos = data;
            });
     
    }

   /* get todos() {
        return this.todoDataService.getAllTodos();
    }
    */
    onAddTodo(todo) {
       /* this.todoDataService
            .addTodo(todo)
            .subscribe(
                (newTodo) => {
                    this.todos = this.todos.push(newTodo);
                }
            );*/
    }

    onToggleTodoComplete(todo) {
    /*    this.todoDataService
            .toggleTodoComplete(todo)
            .subscribe(
                (updatedTodo) => {
                    todo = updatedTodo;
                }
                
            );
            */
    }

    onRemoveTodo(todo) {
/*        this.todoDataService
            .deleteTodoById(todo.id)
            .subscribe(
                (_) => {
                    this.todos = this.todos.filter((t) => t.id !== todo.id);
                }
            );
  */
    }
}