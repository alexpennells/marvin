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
}
