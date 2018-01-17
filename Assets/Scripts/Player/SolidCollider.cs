using UnityEngine;
using System.Collections.Generic;

namespace Player {
  [System.Serializable]
  public class SolidCollider : SolidColliderBlock {
    protected override void WallCollision(SolidObj wall) {
      base.WallCollision(wall);

      if (wall.IsSticky && !Base.HasFooting) {
        if (!Base.SolidPhysics.WallJump.Sliding)
          Base.Sound.Play("Land");

        Base.SolidPhysics.WallJump.StartSliding(wall);
      }

      Base.Physics.hspeed = 0;
    }

    protected override void FootingCollision(SolidObj footing) {
      base.FootingCollision(footing);

      if (SceneStarted && !Base.HasFooting) {
        (Base as Player.Base).CreateWalkPuffs(8);
        Base.Sound.Play("Land");
      }

      Base.Physics.vspeed = 0;
    }
  }
}
