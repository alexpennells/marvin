using UnityEngine;
using System.Collections.Generic;

namespace Bullet {
  public class SolidCollider : SolidColliderObj {
    protected override bool GenericCollision(SolidObj other) {
      Base.State("Impact");
      return false;
    }
  }
}
