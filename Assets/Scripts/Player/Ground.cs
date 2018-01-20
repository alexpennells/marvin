using UnityEngine;
using System;

namespace Player {
  public class Ground : GroundBlock {
    public Ground(GroundBlock old) {
      enabled = true;
      WallJump = old.WallJump;
      startOnGround = old.startOnGround;
    }

    public override void LoadReferences() {
      Collider = new SolidCollider();
      base.LoadReferences();
    }
  }
}
