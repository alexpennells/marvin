using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace PlayerGhost {
  public class Collision : CollisionHandler {
    protected override void HandleCollision(eObjectType otherType, BaseObj other) {
      switch (otherType) {
        case eObjectType.COLLECTOR:
          CollectorCollision(other as Collector.Base);
          break;
      }
    }

    /***********************************
     * HANDLERS
     **********************************/

    private void CollectorCollision(Collector.Base other) {
      PlayerHead.Base head = Game.Create("PlayerHead", other.Mask.Center) as PlayerHead.Base;
      head.State("Revive");
      Game.Camera.LoadFocus();

      other.State("Disappear");
      Base.DestroySelf();
    }
  }
}
