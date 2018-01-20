using UnityEngine;
using System;

namespace Skull {
  public class Sprite : SpriteBlock {
    public Sprite() { enabled = true; }

    public override void Init() {
      Play("Skull");
    }

    public override void Step() {
      float angle = Base.Physics.vspeed * 5;
      FaceAngle(angle);

      if (Base.Physics.hspeed < 0)
        FacingLeft = true;
      else if (Base.Physics.hspeed > 0)
        FacingRight = true;
    }

    public void PlaySkull() {
      Animate("skull", 1f);
    }
  }
}
