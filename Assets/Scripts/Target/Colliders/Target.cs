using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Target {
  namespace Colliders {

    public class Target : CollisionHandler {
      protected override void HandleCollision(eObjectType otherType, BaseObj other) {
        switch (otherType) {
          case eObjectType.WILLY_FIST:
            WillyFistCollision(other as WillyFist.Base);
            break;
        }
      }

      /***********************************
       * HANDLERS
       **********************************/

      private void WillyFistCollision(WillyFist.Base other) {
        other.State("Impact");

        if (Base.Is("Inhaling") || Base.Is("Stretching")) {
          Base.State("Hurt");
        } else {
          Base.State("Break");
        }
      }
    }

  }
}
