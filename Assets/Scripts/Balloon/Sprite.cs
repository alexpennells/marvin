using UnityEngine;
using System;

namespace Balloon {
  public class Sprite : SpriteBlock {
    public override void Init() {
      SetSpeed(0.5f);
    }

    public override void Step() {
      transform.localScale = new Vector3(0.9f + Base.Physics.vspeed / 4, 1f - Base.Physics.vspeed / 2, 1f);
    }
  }
}
