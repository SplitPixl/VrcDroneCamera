import { Component, OnInit, ViewChild } from '@angular/core';
import { DroneConnectionService, DataType } from '../../services/drone-connection.service';
import { Router } from '@angular/router';
import { GamepadService } from './../../services/gamepad.service';
import { IonRange, IonSelect } from '@ionic/angular';
import { Controls } from 'src/app/types/Controls';

@Component({
  selector: 'app-fly',
  templateUrl: './fly.page.html',
  styleUrls: ['./fly.page.scss'],
})
export class FlyPage implements OnInit {
  public running = false;
  public flightInfo = 'N/A';

  constructor(private router: Router, public drone: DroneConnectionService, public gamepad: GamepadService) { }

  ngOnInit() {
    this.start();
  }

  public start() {
    if (!this.running) {
      this.running = true;
      // this.drone.setFlightMode(2);
      this.doUpdate();
    }
  }

  doUpdate() {
    const mnx = this.gamepad.getValue(Controls.moveNegX);
    const mny = this.gamepad.getValue(Controls.moveNegY);
    const mnz = this.gamepad.getValue(Controls.moveNegZ);
    const mpx = this.gamepad.getValue(Controls.movePosX);
    const mpy = this.gamepad.getValue(Controls.movePosY);
    const mpz = this.gamepad.getValue(Controls.movePosZ);

    const rnx = this.gamepad.getValue(Controls.rotateNegX);
    const rny = this.gamepad.getValue(Controls.rotateNegY);
    const rnz = this.gamepad.getValue(Controls.rotateNegZ);
    const rpx = this.gamepad.getValue(Controls.rotatePosX);
    const rpy = this.gamepad.getValue(Controls.rotatePosY);
    const rpz = this.gamepad.getValue(Controls.rotatePosZ);

    const mx = mpx - mnx;
    const my = mpy - mny;
    const mz = mpz - mnz;

    const rx = rpx - rnx;
    const ry = rpy - rny;
    const rz = rpz - rnz;

    // tslint:disable-next-line:max-line-length
    this.flightInfo = `MX: ${mx.toFixed(2)} MY: ${my.toFixed(2)} MZ: ${mz.toFixed(2)} RX: ${rx.toFixed(2)} RY: ${ry.toFixed(2)} RZ: ${rz.toFixed(2)}`;
    this.drone.sendInput(mx, my, mz, rx, ry, rz);

    if (this.running) {
      setTimeout(() => this.doUpdate(), 1000 / 60);
    }

    return;
  }

  public disconnect() {
    this.running = false;
    this.drone.close();
    this.router.navigate(['/connect']);
  }

  public pickController() {
    this.router.navigate(['/controller']);
  }

  public configureController() {
    this.router.navigate(['/controller-config']);
  }
}
