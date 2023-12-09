import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterOutlet } from '@angular/router';
import { stateList } from './mockStates';
import { FormsModule } from '@angular/forms';
import { ChartDataComponent } from './chart-data/chart-data.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, RouterOutlet, FormsModule, ChartDataComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  chartData: any[] = [];
  chartDataKey = 0
  chartComponent = new ChartDataComponent();
  selectedState = '';
  startDate = '';
  endDate = '';
  stateList = stateList;
  title = 'covid-tracker-app';
  showChart = true;

  fetchData() {
    console.log('fetching data');
    const api_input = {
      state: this.selectedState.toLowerCase(),
      startDate: this.startDate,
      endDate: this.endDate
    }
    console.log(api_input);
    fetch(`/api`, {
      method: 'POST',
      headers: { 
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(api_input)  
    })
      .then(response => response.json())
      .then(data => {
        this.chartData = data;
      });
    // fetch(`/api/20200319`)
    // .then(response => response.json())
    // .then(data => {
    //   this.chartData = data;
    // });
  }
}
