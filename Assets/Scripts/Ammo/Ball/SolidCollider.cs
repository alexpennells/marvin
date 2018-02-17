using UnityEngine;
using System;
using System.Collections.Generic;

namespace AmmoBall {
  public class SolidCollider : SolidColliderBlock {
    public SolidCollider() { enabled = true; }

    protected override void WallCollision(SolidObj wall) {
      base.WallCollision(wall);

      Base.Physics.hspeed = -Base.Physics.hspeed / 1.5f;
      Base.Sound.Play("Bounce", Math.Min(0.2f, Math.Abs(Base.Physics.hspeed) / 50));
      Base.Sprite.scaleX = 0.5f;
    }

    protected override void FootingCollision(SolidObj footing) {
      base.FootingCollision(footing);

      if (Base.Physics.vspeed != 0)
        Base.Sound.Play("Bounce", Math.Min(0.2f, Math.Abs(Base.Physics.vspeed) / 50));

      if (Base.Physics.vspeed < -1) {
        Base.Physics.Ground.Collider.ClearFooting();
        Base.Physics.hspeed = Base.Physics.hspeed * 0.9f;
        Base.Physics.vspeed = -Base.Physics.vspeed / 1.25f;
        Base.Sprite.scaleY = 0.5f;
      } else
        Base.Physics.vspeed = 0;

    }
  }
}
