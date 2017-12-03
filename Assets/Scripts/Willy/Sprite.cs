using UnityEngine;
using System;

namespace Willy {
  public class Sprite : SpriteObj {
    public void PlayIdle() {
      Animate("idle", 0.6f);
    }

    public void PlayPunch() {
      Animate("punch", 1f);
    }
  }
}
