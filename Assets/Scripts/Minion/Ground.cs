using UnityEngine;
using System;

namespace Minion {
  public class Ground : GroundBlock {
    public Ground(GroundBlock old) {
      enabled = true;
      startOnGround = old.startOnGround;
    }

    public override void LoadReferences() {
      Collider = new SolidCollider();
      base.LoadReferences();
    }
  }
}
