using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Player {
  public class Collision : CollisionHandler {
    protected override void HandleCollision(eObjectType otherType, BaseObj other) {
      switch (otherType) {
        case eObjectType.LADDER:
          LadderCollision(other as Ladder_Base);
          break;
        case eObjectType.MINION:
          MinionCollision(other as Minion.Base);
          break;
        case eObjectType.SOUL:
          SoulCollision(other as Soul.Base);
          break;
        case eObjectType.BALLOON:
          BalloonCollision(other as Balloon.Base);
          break;
      }
    }

    protected override void HandleExitCollision(eObjectType otherType, BaseObj other) {
      switch (otherType) {
        case eObjectType.LADDER:
          LadderExit(other as Ladder_Base);
          break;
      }
    }

    /***********************************
     * HANDLERS
     **********************************/

    private void LadderCollision(Ladder_Base other) {
      if (!Base.Is("Climbing") && (Game.UpHeld || Game.DownHeld))
        Base.State("Climb", other);
    }

    private void MinionCollision(Minion.Base other) {
      Base.State("Hurt", true);
    }

    private void SoulCollision(Soul.Base other) {
      other.DestroySelf();
    }

    private void BalloonCollision(Balloon.Base other) {
      other.DestroySelf();
    }

    private void LadderExit(Ladder_Base other) {
      if (Base.Is("Climbing") && Base.Physics.Climb.Ladder == other)
        Base.Physics.Climb.Stop();
    }
  }
}
