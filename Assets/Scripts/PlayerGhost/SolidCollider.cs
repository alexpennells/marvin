using UnityEngine;
using System.Collections.Generic;

namespace PlayerGhost {
  public class SolidCollider : SolidColliderObj {
    protected override void WallCollision(SolidObj wall) {
      base.WallCollision(wall);
      Base.Physics.hspeed = 0;
    }

    protected override void FootingCollision(SolidObj footing) {
      base.FootingCollision(footing);
      Base.Physics.vspeed = 0;
    }

    protected override void RoofCollision(SolidObj roof) {
      base.RoofCollision(roof);
      Base.Physics.vspeed = 0;
    }
  }
}
