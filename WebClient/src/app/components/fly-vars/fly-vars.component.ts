import { Component, OnInit } from '@angular/core';
import { DroneConnectionService } from './../../services/drone-connection.service';

@Component({
  selector: 'app-fly-vars',
  templateUrl: './fly-vars.component.html',
  styleUrls: ['./fly-vars.component.scss'],
})
export class FlyVarsComponent implements OnInit {

  constructor(private drone: DroneConnectionService) { }

  ngOnInit() {}

  public updateFloat(id, val) {
    this.drone.updateFloat(id, val);
  }

}
