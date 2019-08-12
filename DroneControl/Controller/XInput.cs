using DroneControl.Types;
using System;
using System.Collections.Generic;
using System.Text;
using XInput.Wrapper;

namespace DroneControl.Controller
{
    class XInput
    {
        XInput. index;
        GamePadState state;

        public XInput(PlayerIndex index = PlayerIndex.One)
        {
            this.index = index;
        }

        public void Update()
        {
            state = GamePad.GetState(index);
        }

        public GamePadState GetState()
        {
            return state;
        }

        public Transform Get3DState()
        {
            return new Transform(
                new Vector3(state.))
        } 
    }
}
