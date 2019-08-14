import { Component, OnInit } from '@angular/core';
import { VrcConnectionService } from './../../services/vrc-connection.service';
import { Router } from '@angular/router';
import { GamepadService } from './../../services/gamepad.service';

@Component({
  selector: 'app-fly',
  templateUrl: './fly.page.html',
  styleUrls: ['./fly.page.scss'],
})
export class FlyPage implements OnInit {
  public running = false;
  public flightInfo = '';

  constructor(private router: Router, public vrc: VrcConnectionService, public gamepad: GamepadService) { }

  ngOnInit() {
    if (!this.vrc.isConnected()) {
      this.router.navigate(['/connect']);
    } else if (!this.gamepad.isConnected()) {
      this.router.navigate(['/gamepad']);
    } else {
      this.running = true;
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
    this.setFlightInfo(`MX: ${mx.toFixed(2)} MY: ${my.toFixed(2)} MZ: ${mz.toFixed(2)} RX: ${rx.toFixed(2)} RY: ${ry.toFixed(2)} RZ: ${rz.toFixed(2)}`);
    // console.log(this.flightInfo);
    this.vrc.sendInput(mx, my, mz, rx, ry, rz);

    setTimeout(() => this.doUpdate(), 1000 / 60);

    return;
  }

  private setFlightInfo(data) {
    this.flightInfo = data;
  }

}
