import { Injectable } from '@angular/core';
import { GamepadConfig } from './../types/GamepadConfig';
import { GamepadInput, InputType } from '../types/GamepadInput';
import { Router } from '@angular/router';
import { Controls } from '../types/Controls';

@Injectable({
  providedIn: 'root'
})
export class GamepadService {

  public padIndex = -1;
  currentConfig: GamepadConfig;

  constructor(private router: Router) { }

  public isConnected() {
    return !!navigator.getGamepads()[this.padIndex];
  }

  public getState() {
    return navigator.getGamepads()[this.getIndex()];
  }

  public setIndex(index) {
    this.padIndex = index;
  }

  public getIndex() {
    if (this.padIndex === -1) {
      throw new Error('Pad Index Not Set!');
    }

    return this.padIndex;
  }

  public getConfig(): GamepadConfig {
    const id = this.getState().id;

    if (!this.currentConfig || this.currentConfig.id !== id) {
      let configs: GamepadConfig[];
      try {
        configs = JSON.parse(localStorage.getItem('gamepadConfigs')) as GamepadConfig[];
        if (!configs) {
          configs = [];
        }
      } catch (err) {
        console.error(err);
        configs = [];
      }

      this.currentConfig = configs.find(config => config.id === id);
    }

    return this.currentConfig;

  }

  public isConfigured() {
    return this.getConfig() !== undefined;
  }

  public async scanInput() {
    let binding: GamepadInput;

    while (!binding) {
      const state = this.getState();
      const pressedButtonIndex = state.buttons.findIndex(b => b.value > 0.75);
      if (pressedButtonIndex >= 0) {
        const input = new GamepadInput();
        input.type = InputType.Button;
        input.index = pressedButtonIndex;
        binding = input;
      }

      const pressedAxisIndex = state.axes.findIndex(a => a < -0.75 || a > 0.75);
      if (pressedAxisIndex >= 0) {
        const input = new GamepadInput();
        if (state.axes[pressedAxisIndex] > 0) {
          input.type = InputType.AxisPositive;
        } else {
          input.type = InputType.AxisNegative;
        }
        input.index = pressedAxisIndex;
        binding = input;
      }

      await new Promise(resolve => setTimeout(resolve, 1000 / 60));
    }

    return binding;
  }

  public getValue(input: Controls) {
    if (!input || this.padIndex === -1) {
      return 0;
    }

    const state = this.getState();
    const config = this.getConfig();

    const requested = config[input];

    if (!requested) {
      return 0;
    }

    let val = 0;

    switch (requested.type) {
      case InputType.Button:
        val = state.buttons[requested.index].value;
        break;
      case InputType.AxisPositive:
        val = Math.max(0, state.axes[requested.index]);
        break;
      case InputType.AxisNegative:
        val =  Math.max(0, -state.axes[requested.index]);
        break;
    }

    if (val > config.deadzone) {
      return val;
    }

    return 0;
  }

  public saveConfig(binding: GamepadConfig) {
    this.currentConfig = binding;
    let configs: GamepadConfig[];
    try {
      configs = JSON.parse(localStorage.getItem('gamepadConfigs')) as GamepadConfig[];
      if (!configs) {
        configs = [];
      }
    } catch (err) {
      console.error(err);
      configs = [];
    }
    const index = configs.findIndex(config => config.id === binding.id);

    if (index === -1) {
      configs.push(binding);
    } else {
      configs[index] = binding;
    }

    localStorage.setItem('gamepadConfigs', JSON.stringify(configs));
  }
}
