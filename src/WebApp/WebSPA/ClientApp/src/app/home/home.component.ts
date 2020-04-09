import { Component } from '@angular/core';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent {
  counter = 0;
  hello(){
    this.counter +=1;
    console.log("hi mario")
  }
}
