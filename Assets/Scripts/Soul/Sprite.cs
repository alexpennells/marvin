using UnityEngine;
using System;

namespace Soul {
  public class Sprite : SpriteObj {
    public override void Init() {
      SetSpeed(0.8f);
    }

    public override void Step() {
      transform.localScale = new Vector3(0.8f + Base.Physics.vspeed, 1f - Base.Physics.vspeed * 2, 1f);
    }
  }
}
