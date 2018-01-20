using UnityEngine;
using System;

namespace Skull {
  public class Ground : GroundBlock {
    public Ground() { enabled = true; }

    public override void LoadReferences() {
      Collider = new SolidCollider();
      base.LoadReferences();
    }
  }
}
