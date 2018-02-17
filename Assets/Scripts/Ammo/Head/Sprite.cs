using UnityEngine;
using System;

namespace AmmoHead {
  public class Sprite : SpriteBlock {
    public Sprite() { enabled = true; }

    public override void Step() {
      if (Base.Physics)
        Base.transform.Rotate(Vector3.forward * -Base.Physics.hspeed * 8);
      else
        Base.transform.Rotate(Vector3.forward * 10);
    }
  }
}
