using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace PlayerGhost {
  public class Collision : CollisionHandler {
    protected override void HandleCollision(eObjectType otherType, BaseObj other) {
      switch (otherType) {
        case eObjectType.COLLECTOR:
          break;
      }
    }

    /***********************************
     * HANDLERS
     **********************************/
  }
}
