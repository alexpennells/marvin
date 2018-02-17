using UnityEngine;
using System;

namespace AmmoSkull {
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
      limitBothVertical = old.limitBothVertical;
      applyFrictionToVspeed = old.applyFrictionToVspeed;
    }

    public override void LoadReferences() {
      Ground = new Ground(Ground);
      base.LoadReferences();
    }
  }
}
