import { Component, OnInit } from '@angular/core';
import { WeatherService } from '../../services/weather.service';
import { FormGroup, FormControl, Validators } from '@angular/forms';

interface IWeather {
  date: string,
  temperatureC: string,
  temperatureF: string,
  summary: string,
}

@Component({
  selector: 'app-weather',
  templateUrl: './weather.component.html',
  styleUrl: './weather.component.scss'
})

export class WeatherComponent implements OnInit {

  public myForm = new FormGroup({
    login: new FormControl('', Validators.required),
    email: new FormControl('', [Validators.required, Validators.email]),
    password: new FormControl('', Validators.required),
  })

  mWeather: IWeather[] = [];

  constructor(private weatherService: WeatherService) { }

  ngOnInit(): void {
    this.GetWeather();
  }

  GetWeather() {
    this.weatherService.getAll().subscribe(str => {
      var _str = JSON.stringify(str);
      var parsedJson = JSON.parse(_str);
      var newWeather = parsedJson;
      this.mWeather = this.mWeather.concat(newWeather);
    });
  }
}
