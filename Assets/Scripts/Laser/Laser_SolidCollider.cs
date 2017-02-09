using UnityEngine;
using System.Collections.Generic;

public class Laser_SolidCollider : SolidColliderObj {
  protected override void WallCollision(SolidObj wall) {
    if (!Base.Is("Impacted"))
      Base.State("Impact");
  }

  protected override void FootingCollision(SolidObj footing) {
    if (!Base.Is("Impacted"))
      Base.State("Impact");
  }

  protected override void RoofCollision(SolidObj roof) {
    if (!Base.Is("Impacted"))
      Base.State("Impact");
  }
}
