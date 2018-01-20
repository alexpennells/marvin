using UnityEngine;
using System;

namespace Door {
  public class Sprite : SpriteBlock {
    public Sprite() { enabled = true; }

    public void PlayIdle() {
      Animate("idle", 1f);
    }

    public void PlayOpen() {
      Animate("open", 1f);
    }

    public void PlayClose() {
      Animate("close", 0f);
    }
  }
}
