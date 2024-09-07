import { Component, OnDestroy, OnInit } from '@angular/core';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss'
})

export class HomeComponent implements OnInit, OnDestroy {

  private audio: HTMLAudioElement;

  constructor() {
    this.audio = new Audio('./../../assets/audio/Untitled1.m4a');
  }

  ngOnInit(): void {
    this.playAudio();
  }

  ngOnDestroy(): void {
    this.stopPlayback();
  }

  playAudio() {
    this.audio.loop = true;
    this.audio.load();
    this.audio.play();
  }

  stopPlayback(): void {
    this.audio.pause();
    this.audio.currentTime = 0;
  }
}