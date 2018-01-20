using UnityEngine;
using System;

namespace Soul {
  public class Ground : GroundBlock {
    public Ground() { enabled = true; }

    public override void LoadReferences() {
      Collider = new SolidCollider();
      base.LoadReferences();
    }
  }
}
