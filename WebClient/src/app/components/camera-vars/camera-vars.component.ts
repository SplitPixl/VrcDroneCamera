import { Component, OnInit, ViewChild } from '@angular/core';
import { IonRange } from '@ionic/angular';
import { DroneConnectionService } from './../../services/drone-connection.service';

@Component({
  selector: 'app-camera-vars',
  templateUrl: './camera-vars.component.html',
  styleUrls: ['./camera-vars.component.scss'],
})
export class CameraVarsComponent implements OnInit {

  fov = 60;
  viewSize = 10;

  @ViewChild('fovSlider', { static: false }) fovSlider: IonRange;
  @ViewChild('projection', { static: false }) projection: IonRange;

  constructor(private drone: DroneConnectionService) { }

  ngOnInit() {}

  public setProjection() {
    this.drone.setProjection(this.projection.value);
  }

  public setFov(val) {
    this.fov = val;
    this.drone.setFov(val);
  }

  public changeFov(val) {
    if (this.fov <= 1 && val < 0) {
      return;
    } else if (this.fov >= 179 && val > 0) {
      return;
    } else {
      this.fov += val;
      this.drone.setFov(this.fov);
    }
  }
}
