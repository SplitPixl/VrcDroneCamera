import { Component, OnInit } from '@angular/core';
import { GamepadService } from './../../services/gamepad.service';
import { Router } from '@angular/router';
import { VrcConnectionService } from './../../services/vrc-connection.service';

@Component({
  selector: 'app-controller',
  templateUrl: './controller.page.html',
  styleUrls: ['./controller.page.scss'],
})
export class ControllerPage implements OnInit {

  public controllers: Gamepad[];

  constructor(private gamepadService: GamepadService, private vrc: VrcConnectionService, private router: Router) { }

  ngOnInit() {
    if (!this.vrc.isConnected()) {
      this.router.navigate(['/connect']);
    }

    this.controllers = navigator.getGamepads();

    window.addEventListener('gamepadconnected', (e) => {
      this.controllers = navigator.getGamepads();
    });

    window.addEventListener('gamepaddisconnected', (e) => {
      this.controllers = navigator.getGamepads();
    });
  }

  public selectController(controller: Gamepad) {
    this.gamepadService.setIndex(controller.index);
    if (this.gamepadService.isConfigured()) {
      this.router.navigate(['/fly']);
    } else {
      this.router.navigate(['/controller-config']);
    }
  }

}
