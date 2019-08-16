import { Component, OnInit } from '@angular/core';
import { DroneConnectionService } from './../../services/drone-connection.service';

@Component({
  selector: 'app-teleport',
  templateUrl: './teleport.component.html',
  styleUrls: ['./teleport.component.scss'],
})
export class TeleportComponent implements OnInit {
  px = 0;
  py = 0;
  pz = 0;
  rx = 0;
  ry = 0;
  rz = 0;

  constructor(private drone: DroneConnectionService) { }

  ngOnInit() { }

  public teleport(form) {
    const px = parseFloat(form.value.px);
    const py = parseFloat(form.value.py);
    const pz = parseFloat(form.value.pz);
    const rx = parseFloat(form.value.rx);
    const ry = parseFloat(form.value.ry);
    const rz = parseFloat(form.value.rz);
    this.drone.teleport(
      px, py, pz,
      rx, ry, rz
    );
  }

}
