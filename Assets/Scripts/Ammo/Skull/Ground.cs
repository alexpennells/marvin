using UnityEngine;
using System;

namespace AmmoSkull {
  public class Ground : GroundBlock {
    public Ground(GroundBlock old) { enabled = true; }

    public override void LoadReferences() {
      Collider = new SolidCollider();
      base.LoadReferences();
    }
  }
}
