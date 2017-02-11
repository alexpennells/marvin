using UnityEngine;
using System.Collections.Generic;

public class Ship_SolidCollider : SolidColliderObj {
  protected override void FootingCollision(SolidObj footing) {
    Base.Physics.vspeed = 3;
  }
}
