using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player_Collision : CollisionHandler {
  protected override void HandleCollision(eObjectType otherType, BaseObj other) {
    switch (otherType) {
      case eObjectType.LADDER:
        LadderCollision(other as Ladder_Base);
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

  private void LadderExit(Ladder_Base other) {
    if (Base.Is("Climbing") && Base.Physics.Climb.Ladder == other)
      Base.Physics.Climb.Stop();
  }
}
