using UnityEngine;
using System;

namespace Skull {
  public class Physics : PhysicsBlock {
    public Physics(PhysicsBlock old) {
      enabled = true;
      Ground = old.Ground;
      hspeedMax = old.hspeedMax;
      vspeedMax = old.vspeedMax;
      hspeed = old.hspeed;
      vspeed = old.vspeed;
      gravity = old.gravity;
      friction = old.friction;
    }

    public override void LoadReferences() {
      Ground = new Ground();
      base.LoadReferences();
    }
  }
}
