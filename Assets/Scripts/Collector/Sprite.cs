using UnityEngine;
using System;

namespace Collector {
  public class Sprite : SpriteBlock {
    public Sprite() { enabled = true; }

    public override void Init() {
      Play("Cut");
    }

    public void PlayCut() {
      Animate("cut", 0f);
    }

    public void PlayHeadlessFly() {
      Animate("headless_fly", 0.5f);
    }

    public void PlayFly() {
      Animate("fly", 0.5f);
    }
  }
}
