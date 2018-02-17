using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace AmmoHead {
  public class Collision : CollisionHandler {
    protected override void HandleCollision(eObjectType otherType, BaseObj other) {
      switch (otherType) {
        case eObjectType.PLAYER:
          PlayerCollision(other as Player.Base);
          break;
      }
    }

    /***********************************
     * HANDLERS
     **********************************/

    private void PlayerCollision(Player.Base other) {
      if (Base.Is("Collectable")) {
        Stitch.AddToAmmo(eAmmoType.HEAD);
        Base.DestroySelf();
      }
    }
  }
}
