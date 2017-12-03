using UnityEngine;
using System.Collections.Generic;

namespace Minion {
  public class SolidCollider : SolidColliderObj {
    protected override void WallCollision(SolidObj wall) {
      base.WallCollision(wall);
      Base.Physics.hspeed = 0;
      (Base as Minion.Base).ChangeDirection();
    }
  }
}