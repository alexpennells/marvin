using UnityEngine;
using System;

namespace PlayerGhost {
  public class Sprite : SpriteBlock {
    public Sprite() { enabled = true; }

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
      else if (Base.Physics.hspeed > 0.5f && FacingRight)
        Play("Forward");
      else if (Base.Physics.hspeed < -0.5f && FacingLeft)
        Play("Forward");
      else
        Play("Reverse");
    }

    public void PlaySpin() {
      Animate("spin", 1.5f);
    }

    public void PlayIdle() {
      Animate("idle", 0.5f);
    }

    public void PlayForward() {
      Animate("forward", 0.75f);
    }

    public void PlayReverse() {
      Animate("reverse", 0.75f);
    }
  }
}
