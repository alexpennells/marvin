using System;

namespace Skull {
  public class SolidPhysics : SolidPhysicsObj {
    public override void LoadReferences() {
      Collider = new SolidCollider();
      Collider.enabled = true;
      base.LoadReferences();
    }
  }
}
