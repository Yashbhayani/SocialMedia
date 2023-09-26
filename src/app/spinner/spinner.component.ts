import { Component } from '@angular/core';
import { LoderService } from '../Services/loder.service';

@Component({
  selector: 'app-spinner',
  templateUrl: './spinner.component.html',
  styleUrls: ['./spinner.component.css'],
})
export class SpinnerComponent {
  constructor(public loader: LoderService) {}
}
