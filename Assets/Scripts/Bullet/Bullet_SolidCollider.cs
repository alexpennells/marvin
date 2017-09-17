using UnityEngine;
using System.Collections.Generic;

public class Bullet_SolidCollider : SolidColliderObj {
  protected override bool GenericCollision(SolidObj other) {
    Base.State("Impact");
    return false;
  }
}
