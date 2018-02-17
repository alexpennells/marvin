using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace AmmoBall {
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
      Stitch.AddToAmmo(eAmmoType.BALL);
      Base.DestroySelf();
    }
  }
}
