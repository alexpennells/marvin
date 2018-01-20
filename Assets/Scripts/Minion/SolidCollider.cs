using UnityEngine;
using System.Collections.Generic;

namespace Minion {
  public class SolidCollider : SolidColliderBlock {
    public SolidCollider() { enabled = true; }

    protected override void WallCollision(SolidObj wall) {
      base.WallCollision(wall);
      Base.Physics.hspeed = 0;
      (Base as Minion.Base).ChangeDirection();
    }

    protected override void FootingCollision(SolidObj footing) {
      base.FootingCollision(footing);

      if (SceneStarted && !Base.HasFooting)
        Base.Sound.Play("Land");

      Base.Physics.vspeed = 0;
    }
  }
}
