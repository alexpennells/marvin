using UnityEngine;
using System;

namespace Minion {
  public class Sprite : SpriteObj {
    public override void Step() {
      if (!Base.Is("Dead")) {
        if (Math.Abs(Base.Physics.hspeed) < 0.5f)
          Play("Idle");
        else
          Play("Walk");
      }

      if (Base.HasFooting)
        FaceFooting();
      else {
        float angle = Base.Physics.vspeed * 2;
        FaceAngle(angle);
      }

      if (!Base.Is("Dead")) {
        if (Base.Physics.hspeed < 0)
          FacingLeft = true;
        else if (Base.Physics.hspeed > 0)
          FacingRight = true;
      }
    }

    public void PlayIdle() {
      Animate("idle", 0.6f);
    }

    public void PlayWalk() {
      Animate("walk", 0.15f);
    }

    public void PlayFall() {
      Animate("fall", 1f);
    }
  }
}
