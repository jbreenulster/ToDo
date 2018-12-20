import { Component } from '@angular/core';
import { Todo } from './_models';
@Component({
    selector: 'app',
    templateUrl: 'app.component.html'
})

export class AppComponent {
    todos: Todo[] = [];
}