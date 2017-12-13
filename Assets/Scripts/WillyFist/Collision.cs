using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace WillyFist {
  public class Collision : CollisionHandler {
    protected override void HandleCollision(eObjectType otherType, BaseObj other) {
      switch (otherType) {
        case eObjectType.BALLOON:
          BalloonCollision(other as Balloon.Base);
          break;
      }
    }

    /***********************************
     * HANDLERS
     **********************************/

    private void BalloonCollision(Balloon.Base other) {
      other.DestroySelf();
    }
  }
}
