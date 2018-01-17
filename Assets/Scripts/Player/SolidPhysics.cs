using System;

namespace Player {
  public class SolidPhysics : SolidPhysicsObj {
    public override void LoadReferences() {
      Collider = new SolidCollider();
      Collider.enabled = true;
      base.LoadReferences();
    }
  }
}
