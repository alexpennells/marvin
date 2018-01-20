using UnityEngine;
using System;

namespace PlayerHead {
  public class Sprite : SpriteBlock {
    public Sprite() { enabled = true; }
    private float angle = 0f;

    public override void Step() {
      if (IsPlaying("revive"))
        SetAngle(0f);
      else {
        angle += FacingLeft ? -20 : 20;
        SetAngle(angle);
      }
    }

    public void PlayDead() {
      Animate("dead", 0f);
    }

    public void PlayAlive() {
      Animate("alive", 0f);
    }

    public void PlayRevive() {
      Animate("revive", 0.4f);
    }
  }
}
