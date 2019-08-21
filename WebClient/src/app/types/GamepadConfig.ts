import { GamepadInput } from './GamepadInput';

export class GamepadConfig {
  public static PrettyNames = {
    movePosX: 'Move Right',
    moveNegX: 'Move Left',
    movePosY: 'Move Up',
    moveNegY: 'Move Down',
    movePosZ: 'Move Forward',
    moveNegZ: 'Move Backward',

    rotatePosX: 'Look Down',
    rotateNegX: 'Look Up',
    rotatePosY: 'Look Right',
    rotateNegY: 'Look Left',
    rotatePosZ: 'Rotate Counterclockwise',
    rotateNegZ: 'Rotate Clockwise',

    fovPos: 'Increase FOV (Zoom Out)',
    fovNeg: 'Decrease FOV (Zoom In)',
  }

  public id: string;

  public movePosX: GamepadInput;
  public moveNegX: GamepadInput;
  public movePosY: GamepadInput;
  public moveNegY: GamepadInput;
  public movePosZ: GamepadInput;
  public moveNegZ: GamepadInput;

  public rotatePosX: GamepadInput;
  public rotateNegX: GamepadInput;
  public rotatePosY: GamepadInput;
  public rotateNegY: GamepadInput;
  public rotatePosZ: GamepadInput;
  public rotateNegZ: GamepadInput;

  public fovPos: GamepadInput;
  public fovNeg: GamepadInput;

  public reset: GamepadInput;

  public deadzone = 0.05;
  public fovSpeed = 1;
}
