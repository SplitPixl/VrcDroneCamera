import { Component, OnInit } from '@angular/core';
import { DroneConnectionService } from './../../services/drone-connection.service';

@Component({
  selector: 'app-teleport',
  templateUrl: './teleport.component.html',
  styleUrls: ['./teleport.component.scss'],
})
export class TeleportComponent implements OnInit {

  constructor(private drone: DroneConnectionService) { }

  ngOnInit() { }

  public teleport(form) {
    const px = parseFloat(form.value.px);
    const py = parseFloat(form.value.py);
    const pz = parseFloat(form.value.pz);
    const rx = parseFloat(form.value.rx);
    const ry = parseFloat(form.value.ry);
    const rz = parseFloat(form.value.rz);
    console.log(form.value.px, form.value.py, form.value.pz);
    console.log(form.value.rx, form.value.ry, form.value.rz);
    console.log(px, py, pz);
    console.log(rx, ry, rz);
    console.log(form);
    this.drone.teleport(
      px, py, pz,
      rx, ry, rz
    );
  }

}
