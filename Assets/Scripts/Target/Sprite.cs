using UnityEngine;
using System;

namespace Target {
  public class Sprite : SpriteBlock {
    public void PlayIdle() {
      Animate("idle", 1f);
    }

    public void PlayStretch() {
      Animate("stretch", 1f);
    }

    public void PlayShrink() {
      Animate("shrink", 0.5f);
    }

    public void PlayInhale() {
      Animate("inhale", 1f);
    }

    public void PlayBreak() {
      Animate("break", 1.2f);
    }
  }
}
