import { Component, OnInit, ViewChild } from '@angular/core';
import { IonRange } from '@ionic/angular';
import { DroneConnectionService } from './../../services/drone-connection.service';

@Component({
  selector: 'app-camera-vars',
  templateUrl: './camera-vars.component.html',
  styleUrls: ['./camera-vars.component.scss'],
})
export class CameraVarsComponent implements OnInit {

  @ViewChild('fovSlider', { static: false }) fovSlider: IonRange;
  @ViewChild('projection', { static: false }) projection: IonRange;

  constructor(private drone: DroneConnectionService) { }

  ngOnInit() {}

  public setFov(val) {
    console.log(this.fovSlider.value);
    this.drone.setFov(this.fovSlider.value);
  }
}
