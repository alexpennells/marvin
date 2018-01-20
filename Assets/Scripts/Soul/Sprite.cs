using UnityEngine;
using System;

namespace Soul {
  public class Sprite : SpriteBlock {
    public Sprite() { enabled = true; }

    public override void Init() {
      SetSpeed(0.8f);
    }
  }
}
