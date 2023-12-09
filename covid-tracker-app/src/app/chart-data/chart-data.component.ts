import { CommonModule } from '@angular/common';
import { Component, Input, OnChanges, SimpleChanges } from '@angular/core';
import { ChartDataset, ChartOptions, ChartConfiguration, plugins } from 'chart.js';
import { NgChartsModule } from 'ng2-charts';
import { covidData } from '../mockCovidData'; 
import { HtmlParser } from '@angular/compiler';


@Component({
  standalone: true,
  selector: 'app-chart-data',
  templateUrl: './chart-data.component.html',
  imports: [NgChartsModule, CommonModule],
  styleUrls: ['./chart-data.component.css']
})

export class ChartDataComponent implements OnChanges {
  @Input() chartData: any[] = [];
  barChartLabels: string[] = [];
  public barChartData: ChartDataset[] = [
    { data: [], label: 'Positive' },
    { data: [], label: 'Negative'}
  ];


  // ... existing component code ...

  ngOnChanges(changes: SimpleChanges) {
    if (changes['chartData']) {
      this.updateChart(changes['chartData'].currentValue);
    }
  }

  updateChart(data: any[]) {
    // Clear existing data
    this.barChartLabels = [];
    this.barChartData[0].data = [];
    this.barChartData[1].data = [];

    // Process new data
    data.forEach(item => {
      this.barChartLabels.push(item.date);
      this.barChartData[0].data?.push(+item.positive);
      this.barChartData[1].data?.push(+item.negative);
    });
  }
}
