import { Component, OnInit } from '@angular/core';
import { DroneConnectionService, DataType } from '../../services/drone-connection.service';
import { Router } from '@angular/router';
import { GamepadService } from './../../services/gamepad.service';

@Component({
  selector: 'app-fly',
  templateUrl: './fly.page.html',
  styleUrls: ['./fly.page.scss'],
})
export class FlyPage implements OnInit {
  public running = false;
  public flightInfo = 'N/A';

  constructor(private router: Router, public drone: DroneConnectionService, public gamepad: GamepadService) {
    if (!drone.isConnected()) {
      router.navigate(['/connect']);
    } else if (!this.gamepad.isConnected()) {
      router.navigate(['/gamepad']);
    } else {

    }
  }

  ngOnInit() {
    this.start();
  }

  public start() {
    if (!this.running) {
      this.running = true;
      this.drone.setFlightMode(2);
      this.doUpdate();
    }
  }

  doUpdate() {
    const config = this.gamepad.getConfig();

    const mnx = this.gamepad.readBindingValue(config.moveNegX);
    const mny = this.gamepad.readBindingValue(config.moveNegY);
    const mnz = this.gamepad.readBindingValue(config.moveNegZ);
    const mpx = this.gamepad.readBindingValue(config.movePosX);
    const mpy = this.gamepad.readBindingValue(config.movePosY);
    const mpz = this.gamepad.readBindingValue(config.movePosZ);

    const rnx = this.gamepad.readBindingValue(config.rotateNegX);
    const rny = this.gamepad.readBindingValue(config.rotateNegY);
    const rnz = this.gamepad.readBindingValue(config.rotateNegZ);
    const rpx = this.gamepad.readBindingValue(config.rotatePosX);
    const rpy = this.gamepad.readBindingValue(config.rotatePosY);
    const rpz = this.gamepad.readBindingValue(config.rotatePosZ);

    const mx = mpx - mnx;
    const my = mpy - mny;
    const mz = mpz - mnz;

    const rx = rpx - rnx;
    const ry = rpy - rny;
    const rz = rpz - rnz;

    // tslint:disable-next-line:max-line-length
    this.flightInfo = (`MX: ${mx.toFixed(2)} MY: ${my.toFixed(2)} MZ: ${mz.toFixed(2)} RX: ${rx.toFixed(2)} RY: ${ry.toFixed(2)} RZ: ${rz.toFixed(2)}`);
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

  public configureController() {
    this.router.navigate(['/controller-config']);
  }

  public teleport(form) {
    this.drone.teleport(
      parseFloat(form.value.px),
      parseFloat(form.value.py),
      parseFloat(form.value.pz),
      parseFloat(form.value.rx),
      parseFloat(form.value.ry),
      parseFloat(form.value.rz)
    );
  }

  public updateFloat(id, val) {
    this.drone.updateFloat(id, val);
  }

}
