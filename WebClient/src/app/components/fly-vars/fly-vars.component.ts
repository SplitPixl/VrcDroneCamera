import { Component, OnInit, ViewChild } from '@angular/core';
import { DroneConnectionService } from './../../services/drone-connection.service';
import { THIS_EXPR } from '@angular/compiler/src/output/output_ast';

@Component({
  selector: 'app-fly-vars',
  templateUrl: './fly-vars.component.html',
  styleUrls: ['./fly-vars.component.scss'],
})
export class FlyVarsComponent implements OnInit {
  public moveSpeed = 2;
  public rotSpeed = 90;
  public moveSmooth = 10;
  public rotSmooth = 10;

  @ViewChild('flightMode', { static: false }) flightMode;

  constructor(private drone: DroneConnectionService) { }

  ngOnInit() {}

  public setFlightMode() {
    this.drone.setFlightMode(this.flightMode.value);
  }

  public updateFloat(id, val) {
    this.drone.updateFloat(id, val);
  }

}
