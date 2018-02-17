using UnityEngine;
using System;

namespace Bullet {
  public class Sprite : SpriteBlock {
    public Sprite() { enabled = true; }

    public override void Init() {
      FacingLeft = Base.Physics.hspeed < 0;
    }

    public override void Step() {
      Base.transform.Rotate(Vector3.forward * Time.deltaTime * -1000, Space.World);
    }
  }
}
