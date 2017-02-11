using UnityEngine;
using System.Collections.Generic;

public class Torpedo_SolidCollider : SolidColliderObj {
  protected override bool GenericCollision(SolidObj other) {
    Base.State("Impact");
    return false;
  }
}
