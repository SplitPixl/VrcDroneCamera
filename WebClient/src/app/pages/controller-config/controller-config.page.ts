import { Component, OnInit } from '@angular/core';
import { PopoverController } from '@ionic/angular';
import { BindInputComponent } from './../../components/bind-input/bind-input.component';
import { GamepadConfig } from 'src/app/types/GamepadConfig';
import { GamepadService } from './../../services/gamepad.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-controller-config',
  templateUrl: './controller-config.page.html',
  styleUrls: ['./controller-config.page.scss'],
})
export class ControllerConfigPage implements OnInit {
  config: GamepadConfig;
  controller;

  constructor(public popoverController: PopoverController, private gamepadService: GamepadService, private router: Router) { }

  ngOnInit() {
    try {
      this.controller = this.gamepadService.getState().id;

      this.config = this.gamepadService.getConfig();
      if (!this.config) {
        this.config = new GamepadConfig();
        this.config.id = this.gamepadService.getState().id;
      }
    } catch (err) {
      console.error(err);
      setTimeout(() => {
        this.router.navigate(['/connect']);
      }, 1000);
    }
  }

  public bindName(name) {
    return GamepadConfig.PrettyNames[name];
  }

  public async bindInput(name) {
    const popover = await this.popoverController.create({
      component: BindInputComponent,
      componentProps: {
        input: name,
        bindings: this.config
      },
      translucent: true
    });
    return await popover.present();
  }

  public save() {
    this.gamepadService.saveConfig(this.config);
    this.router.navigate(['/fly']);
  }

}
