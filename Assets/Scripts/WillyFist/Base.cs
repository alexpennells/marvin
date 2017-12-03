using UnityEngine;
using System;

namespace WillyFist {
  [RequireComponent (typeof (Sprite))]
  public class Base : BaseObj {
    private bool moveUp = false;

    protected override void Init() {
      Sprite.StartBlur(0.025f, 1f, 0, 0.16f);
    }

    protected override void Step() {
      if (moveUp)
        Physics.vspeed += 0.1f;
      else
        Physics.vspeed -= 0.1f;

      if (Physics.vspeed > 1f)
        moveUp = false;
      else if (Physics.vspeed < -1f)
        moveUp = true;

      // Rotate to face position.
      float angle = Physics.vspeed * 12f;
      if (Sprite.FacingLeft)
        angle *= -1;
      Sprite.FaceAngle(angle);
    }

    protected override void OffScreenStep() {
      DestroySelf();
    }

    /***********************************
     * STATE CHANGE FUNCTIONS
     **********************************/

    public void StateImpact() {
      Game.CreateParticle("WillyFistPuff", Position);
      Sprite.StopBlur();
      DestroySelf();
    }
  }
}
