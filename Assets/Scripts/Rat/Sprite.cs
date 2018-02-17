using UnityEngine;
using System;

namespace Rat {
  public class Sprite : SpriteBlock {
    public Sprite() { enabled = true; }

    public override void Init() {
      Play("Walk");
    }

    public override void Step() {
      if (Base.Is("Dead")) {
        if (FacingRight)
          FaceAngle(currentRotationAngle + 1f);
        else
          FaceAngle(currentRotationAngle - 1f);
        return;
      }

      if (Base.HasFooting)
        FaceFooting();
      else {
        float angle = Base.Physics.vspeed * 2;
        FaceAngle(angle);
      }

      if (Base.Physics.hspeed < 0)
        FacingLeft = true;
      else if (Base.Physics.hspeed > 0)
        FacingRight = true;
    }

    public void PlayWalk() {
      Animate("walk", 0.6f);
    }

    public void PlayDie() {
      Animate("die", 0.5f);
    }
  }
}
