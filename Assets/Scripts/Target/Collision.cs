using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Target {
  public class Collision : CollisionHandler {
    protected override void HandleCollision(eObjectType otherType, BaseObj other) {
      switch (otherType) {
        case eObjectType.BULLET:
          BulletCollision(other as Bullet.Base);
          break;
      }
    }

    /***********************************
     * HANDLERS
     **********************************/

    private void BulletCollision(Bullet.Base other) {
      if (other.impacted)
        return;

      other.impacted = true;
      if (Base.Is("Inhaling") || Base.Is("Stretching")) {
        Base.State("Hurt");
      } else {
        Base.State("Break");
      }
    }
  }
}
