using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpeechBubble_Collision : CollisionStubs {
  public override eObjectType Type { get { return eObjectType.SPEECH_BUBBLE; } }

  protected override void PlayerCollision(Player_Base other) {
    Base.Sprite.Play("Open");

    if (Game.UpHeld && other.HasFooting) {
      Base.State("PlayText");
      other.Physics.hspeed = 0;
      other.Sprite.Play("Idle");
    }
  }

  protected override void PlayerExit(Player_Base other) {
    Base.Sprite.Play("Close");
  }
}
