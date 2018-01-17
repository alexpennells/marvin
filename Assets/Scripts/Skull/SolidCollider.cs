using UnityEngine;
using System.Collections.Generic;

namespace Skull {
  public class SolidCollider : SolidColliderBlock {
    protected override void WallCollision(SolidObj wall) {
      base.WallCollision(wall);
      Base.Physics.hspeed = 0;
      (Base as Skull.Base).ChangeDirection();
    }

    protected override void FootingCollision(SolidObj footing) {
      base.FootingCollision(footing);
      Base.Physics.vspeed = 0;

      if (!Base.HasFooting) {
        Base.Sound.Play("Land");
        (Base as Skull.Base).Jump();
        (Base as Skull.Base).CreateWalkPuffs(2);
      }
    }
  }
}
