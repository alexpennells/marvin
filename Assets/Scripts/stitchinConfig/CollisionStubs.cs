// Since C# is shitty at generic types, the object types need to be defined
// per project to create simple stubs for them at the collision level.

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CollisionStubs : CollisionObj
{
  /***********************************
   * FUNCTIONS
   **********************************/

  // Event handler that gets fired from the CollisionHandler instance attached to the BoxCollider/Rigidbody.
  new public virtual void HandleCollision(BaseObj other) {
    switch (other.Collision.Type) {
      case eObjectType.PLAYER:
        PlayerCollision(other as Player_Base);
        break;
      case eObjectType.LADDER:
        LadderCollision(other as Ladder_Base);
        break;
      case eObjectType.WATER:
        WaterCollision(other as Water_Base);
        break;
      case eObjectType.LASER:
        LaserCollision(other as Laser_Base);
        break;
      case eObjectType.TORPEDO:
        TorpedoCollision(other as Torpedo_Base);
        break;
      case eObjectType.SHIP:
        ShipCollision(other as Ship_Base);
        break;
    }
  }

  new public virtual void HandleExitCollision(BaseObj other) {
    switch (other.Collision.Type) {
      case eObjectType.PLAYER:
        PlayerExit(other as Player_Base);
        break;
      case eObjectType.LADDER:
        LadderExit(other as Ladder_Base);
        break;
      case eObjectType.WATER:
        WaterExit(other as Water_Base);
        break;
    }
  }

  /***********************************
   * FUNCTION STUBS
   **********************************/

  protected virtual void PlayerCollision(Player_Base other) {}
  protected virtual void LadderCollision(Ladder_Base other) {}
  protected virtual void WaterCollision(Water_Base other) {}
  protected virtual void LaserCollision(Laser_Base other) {}
  protected virtual void TorpedoCollision(Torpedo_Base other) {}
  protected virtual void ShipCollision(Ship_Base other) {}
  protected virtual void PlayerExit(Player_Base other) {}
  protected virtual void LadderExit(Ladder_Base other) {}
  protected virtual void WaterExit(Water_Base other) {}
}
