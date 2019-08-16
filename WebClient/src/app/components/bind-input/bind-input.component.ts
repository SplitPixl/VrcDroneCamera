import { Component, OnInit, Input } from '@angular/core';
import { GamepadConfig } from './../../types/GamepadConfig';
import { GamepadService } from 'src/app/services/gamepad.service';
import { InputType } from 'src/app/types/GamepadInput';
import { PopoverController } from '@ionic/angular';

@Component({
  selector: 'app-bind-input',
  templateUrl: './bind-input.component.html',
  styleUrls: ['./bind-input.component.scss'],
})
export class BindInputComponent implements OnInit {
  @Input() input: string;
  @Input() bindings: GamepadConfig;

  private waitingForBind = false;

  constructor(private gamepadService: GamepadService, private popoverController: PopoverController) { }

  ngOnInit() {
    if (!this.bindings[this.input]) {
      this.bind();
    }
  }

  public bindName(name) {
    return GamepadConfig.PrettyNames[name];
  }

  public getTypeName(type) {
    return InputType[type];
  }

  public async bind() {
    this.waitingForBind = true;
    const theInput = await this.gamepadService.scanInput();
    if (this.waitingForBind) {
      this.bindings[this.input] = theInput;
    }
    this.waitingForBind = false;
  }

  public clear() {
    this.waitingForBind = false;
    this.bindings[this.input] = null;
  }

  public close() {
    this.waitingForBind = false;
    this.popoverController.dismiss();
  }

  public clearAndClose() {
    this.clear();
    this.close();
  }
}
