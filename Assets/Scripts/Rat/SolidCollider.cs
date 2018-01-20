using UnityEngine;
using System.Collections.Generic;

namespace Rat {
  public class SolidCollider : SolidColliderBlock {
    public SolidCollider() { enabled = true; }

    protected override void WallCollision(SolidObj wall) {
      base.WallCollision(wall);
      Base.Physics.hspeed = 0;
      (Base as Rat.Base).ChangeDirection();
    }

    protected override void FootingCollision(SolidObj footing) {
      base.FootingCollision(footing);
      Base.Physics.vspeed = 0;
    }
  }
}
