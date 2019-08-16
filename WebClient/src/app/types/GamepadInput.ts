export enum InputType {
  Button,
  AxisPositive,
  AxisNegative
}

export class GamepadInput {
  public type: InputType;
  public index: number;
}