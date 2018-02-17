using UnityEngine;
using System;

namespace Player {
  public class Physics : PhysicsBlock {
    public Physics(PhysicsBlock old) {
      enabled = true;
      Rail = old.Rail;
      Ground = old.Ground;
      hspeedMax = old.hspeedMax;
      vspeedMax = old.vspeedMax;
      hspeed = old.hspeed;
      vspeed = old.vspeed;
      gravity = old.gravity;
      friction = old.friction;
    }

    public override void LoadReferences() {
      Ground = new Ground(Ground);
      base.LoadReferences();
    }

    public override void Step() {
      if (Base.Is("Pounding"))
        SkipNextGravityUpdate();

      base.Step();
    }
  }
}
