import { Component, Input } from '@angular/core';
import { Todo } from '../_models';

@Component({
  selector: 'app-todo-list-footer',
  templateUrl: 'todo-list-footer.component.html',
  styleUrls: ['todo-list-footer.component.css']
})
export class TodoListFooterComponent {

  @Input()
  todos: Todo[];

  constructor() {
  }

}
