using UnityEngine;
using System;

namespace AmmoSkull {
  public class Sprite : SpriteBlock {
    public Sprite() { enabled = true; }

    public override void Step() {
      Base.transform.Rotate(Vector3.forward * -Base.Physics.hspeed * 8);
    }
  }
}
