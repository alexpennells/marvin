using UnityEngine;
using System;
using System.Collections;

namespace PlayerGhost {
  [RequireComponent (typeof (Sprite))]
  [RequireComponent (typeof (Sound))]
  public class Base : InputObj {
    [Tooltip("The amount of speed the player picks up per step")]
    public Vector2 accelerationSpeed = new Vector2(0.1f, 0.05f);

    protected override void LeftHeld (float val) {
      Physics.hspeed -= accelerationSpeed.x;
    }

    protected override void RightHeld (float val) {
      Physics.hspeed += accelerationSpeed.x;
    }

    protected override void UpHeld (float val) {
      Physics.vspeed += accelerationSpeed.y;
    }

    protected override void DownHeld (float val) {
      Physics.vspeed -= accelerationSpeed.y;
    }
  }
}
