using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Torpedo_Collision : CollisionStubs
{
  public override eObjectType Type { get { return eObjectType.TORPEDO; } }

  protected override void WormCollision(Worm_Base other) {
    Base.State("Impact");
    other.State("Hurt", 3);
  }

  protected override void WormBodyCollision(WormBody_Base other) {
    Base.State("Impact");
  }
}
