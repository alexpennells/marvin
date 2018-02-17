using UnityEngine;
using System;

namespace AmmoBall {
  public class Sprite : SpriteBlock {
    public Sprite() { enabled = true; }

    public override void Step() {
      base.Step();
      UpdateScaleX();
      UpdateScaleY();
    }

    private void UpdateScaleX() {
      if (1 < scaleX)
        scaleX = Math.Max(1, scaleX - 0.1f);
      else if (1 > scaleX)
        scaleX = Math.Min(1, scaleX + 0.1f);
    }

    private void UpdateScaleY() {
      float desired = 1 + Math.Abs(Base.Physics.vspeed) / 8;

      if (desired < scaleY)
        scaleY = Math.Max(desired, scaleY - 0.1f);
      else if (desired > scaleY)
        scaleY = Math.Min(desired, scaleY + 0.1f);
    }

  }
}
