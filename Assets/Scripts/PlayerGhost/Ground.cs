using UnityEngine;
using System;

namespace PlayerGhost {
  public class Ground : GroundBlock {
    public Ground() { enabled = true; }

    public override void LoadReferences() {
      Collider = new SolidCollider();
      base.LoadReferences();
    }
  }
}
