using UnityEngine;
using System;

namespace Skull {
  public class Sprite : SpriteBlock {
    public Sprite() { enabled = true; }

    public override void Init() {
      Play("Skull");
    }

    public override void Step() {
      // Adjust y scale based on vspeed
      float yScale = Math.Max(1 + Base.Physics.vspeed / 10, 0.5f);
      transform.localScale = new Vector3(transform.localScale.x, yScale, 1);

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
