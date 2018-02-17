using UnityEngine;
using System.Collections.Generic;

namespace Player {
  [System.Serializable]
  public class SolidCollider : SolidColliderBlock {
    public SolidCollider() { enabled = true; }

    protected override void WallCollision(SolidObj wall) {
      base.WallCollision(wall);

      if (wall.IsSticky && !Base.HasFooting) {
        if (!Base.Physics.Ground.WallJump.Sliding)
          Base.Sound.Play("Land");

        Base.Physics.Ground.WallJump.StartSliding(wall);
      }

      Base.Physics.hspeed = 0;
    }

    protected override void FootingCollision(SolidObj footing) {
      base.FootingCollision(footing);

      if (Base.Is("Pounding") && Base.Physics.vspeed != 0) {
        Base.StartCoroutine("GroundPound");
        if (footing.transform.parent.gameObject.name == "DestroyableGrave")
          footing.State("Damage");
      } else if (SceneStarted && !Base.HasFooting) {
        (Base as Player.Base).CreateWalkPuffs(8);
        Base.Sound.Play("Land");
      }

      Base.Physics.vspeed = 0;
    }
  }
}
