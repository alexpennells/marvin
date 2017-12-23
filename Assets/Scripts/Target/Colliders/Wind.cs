using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Target {
  namespace Colliders {

    public class Wind : CollisionHandler {
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
        if (Base.Is("Inhaling")) {
          other.Physics.SkipNextFrictionUpdate();
          other.Physics.hspeed += 0.1f;
        }
      }
    }

  }
}
