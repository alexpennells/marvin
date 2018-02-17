using UnityEngine;
using System;
using System.Collections.Generic;

namespace AmmoSkull {
  public class SolidCollider : SolidColliderBlock {
    public SolidCollider() { enabled = true; }

    protected override void WallCollision(SolidObj wall) {
      base.WallCollision(wall);
      Base.Physics.hspeed = 0;
    }

    protected override void FootingCollision(SolidObj footing) {
      base.FootingCollision(footing);
      if (Base.Physics.vspeed != 0) { Base.Sound.Play("Land"); }
      Base.Physics.vspeed = 0;
    }
  }
}
