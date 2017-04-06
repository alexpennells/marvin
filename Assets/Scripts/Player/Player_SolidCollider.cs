using UnityEngine;
using System.Collections.Generic;

public class Player_SolidCollider : SolidColliderObj {
  protected override bool GenericCollision(SolidObj other) {
    if (Base.Is("Pouncing")) {
      Base.Sprite.StopBlur();
      Base.Sprite.Play("Idle");
    }

    return true;
  }

  protected override void WallCollision(SolidObj wall) {
    base.WallCollision(wall);

    Base.Physics.hspeed = 0;
    if (Base.SolidPhysics.Walljump.Sliding)
      (Base as Player_Base).CanDoubleJump = true;
  }
}
