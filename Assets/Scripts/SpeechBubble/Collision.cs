using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SpeechBubble {
  public class Collision : CollisionHandler {
    protected override void HandleCollision(eObjectType otherType, BaseObj other) {
      switch (otherType) {
        case eObjectType.PLAYER:
          PlayerCollision(other as Player_Base);
          break;
      }
    }

    protected override void HandleExitCollision(eObjectType otherType, BaseObj other) {
      switch (otherType) {
        case eObjectType.PLAYER:
          PlayerExit(other as Player_Base);
          break;
      }
    }

    /***********************************
     * HANDLERS
     **********************************/

    private void PlayerCollision(Player_Base other) {
      Base.Sprite.Play("Open");

      if (Game.UpHeld && other.HasFooting) {
        Base.State("PlayText");
        other.Physics.hspeed = 0;
        other.Sprite.Play("Idle");
      }
    }

    private void PlayerExit(Player_Base other) {
      Base.Sprite.Play("Close");
    }
  }
}
