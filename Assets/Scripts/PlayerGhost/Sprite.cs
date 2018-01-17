using UnityEngine;
using System;

namespace PlayerGhost {
  public class Sprite : SpriteBlock {
    public override void Init() {
      Play("Spin");
    }

    public override void Step() {
      base.Step();

      float angle = Base.Physics.vspeed * 4;
      if (FacingLeft)
        angle *= -1;
      FaceAngle(angle);

      if (Base.Is("Spinning")) {
        if (Base.Physics.hspeed > 0)
          FacingLeft = true;
        else if (Base.Physics.hspeed < 0)
          FacingRight = true;
      }
      else if (Base.Physics.hspeed > 0.5f) {
        Play(Game.RightHeld ? "Forward" : "Backward");
        FacingRight = true;
      } else if (Base.Physics.hspeed < -0.5f) {
        Play(Game.LeftHeld ? "Forward" : "Backward");
        FacingLeft = true;
      }
      else
        Play("Idle");
    }

    public void PlaySpin() {
      Animate("ghost_spin", 1.5f);
    }

    public void PlayIdle() {
      Animate("ghost_idle", 0.5f);
    }

    public void PlayForward() {
      Animate("ghost_forward", 0.75f);
    }

    public void PlayBackward() {
      Animate("ghost_backward", 0.75f);
    }
  }
}
