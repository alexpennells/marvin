using UnityEngine;
using System.Collections.Generic;

public class Player_SolidCollider : SolidColliderObj {
  protected override bool GenericCollision(SolidObj other) {
    return true;
  }

  protected override void WallCollision(SolidObj wall) {
    base.WallCollision(wall);

    if (wall.IsSticky && !Base.HasFooting) {
      if (!Base.SolidPhysics.Walljump.Sliding)
        Base.Sound.Play("Land");

      Base.SolidPhysics.Walljump.StartSliding(wall);
    }

    Base.Physics.hspeed = 0;
  }

  protected override void FootingCollision(SolidObj footing) {
    base.FootingCollision(footing);

    if (!Base.HasFooting)
      Base.Sound.Play("Land");

    Base.Physics.vspeed = 0;
  }
}
