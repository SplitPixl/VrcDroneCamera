import { Injectable } from '@angular/core';
import { GamepadConfig } from './../types/GamepadConfig';
import { GamepadInput, InputType } from '../types/GamepadInput';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class GamepadService {

  padIndex = -1;
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

  public async readOneInput() {
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

      await new Promise(resolve => setTimeout(resolve, 50));
    }

    return binding;
  }

  public readBindingValue(input: GamepadInput) {
    if (!input) {
      return 0;
    }

    const state = this.getState();

    switch (input.type) {
      case InputType.Button:
        return state.buttons[input.index].value;
      case InputType.AxisPositive:
        return Math.max(0, state.axes[input.index]);
      case InputType.AxisNegative:
        return Math.max(0, -state.axes[input.index]);
    }
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
