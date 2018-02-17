using UnityEngine;
using System;

namespace AmmoHead {
  public class Ground : GroundBlock {
    public Ground(GroundBlock old) { enabled = true; }

    public override void LoadReferences() {
      Collider = new SolidCollider();
      base.LoadReferences();
    }
  }
}
