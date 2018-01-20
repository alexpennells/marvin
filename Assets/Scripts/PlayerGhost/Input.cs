using UnityEngine;
using System;

namespace PlayerGhost {
  public class Input : InputBlock {
    public Input() { enabled = true; }

    protected override void LeftHeld (float val) {
      if (!Base.Is("Spinning"))
        Base.Physics.hspeed -= (Base as PlayerGhost.Base).accelerationSpeed.x;
    }

    protected override void RightHeld (float val) {
      if (!Base.Is("Spinning"))
        Base.Physics.hspeed += (Base as PlayerGhost.Base).accelerationSpeed.x;
    }

    protected override void UpHeld (float val) {
      if (!Base.Is("Spinning"))
        Base.Physics.vspeed += (Base as PlayerGhost.Base).accelerationSpeed.y;
    }

    protected override void DownHeld (float val) {
      if (!Base.Is("Spinning"))
        Base.Physics.vspeed -= (Base as PlayerGhost.Base).accelerationSpeed.y;
    }
  }
}
