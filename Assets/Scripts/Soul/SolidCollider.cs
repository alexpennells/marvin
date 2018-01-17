using UnityEngine;
using System.Collections.Generic;

namespace Soul {
  public class SolidCollider : SolidColliderBlock {
    protected override void WallCollision(SolidObj wall) {
      base.WallCollision(wall);
      Base.Physics.hspeed = 0;
    }

    protected override void FootingCollision(SolidObj footing) {
      Base.Physics.vspeed = 0;
    }
  }
}
