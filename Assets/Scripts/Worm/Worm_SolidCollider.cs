using UnityEngine;
using System.Collections.Generic;

public class Worm_SolidCollider : SolidColliderObj {
  protected override void FootingCollision(SolidObj footing) {
    Base.Physics.vspeed = Base.Physics.vspeedMax;
  }

  protected override void WallCollision(SolidObj wall) {
    Base.Physics.hspeed = -Base.Physics.hspeed;
  }
}
