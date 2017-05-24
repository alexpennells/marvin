using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Laser_Collision : CollisionStubs
{
  public override eObjectType Type { get { return eObjectType.LASER; } }

  protected override void WaterCollision(Water_Base other) {
    if (!Base.Is("Submerged"))
      Base.State("Submerged");
  }

  protected override void WaterExit(Water_Base other) {
    if (Base.Is("Submerged"))
      Base.State("Airborne");
  }

  protected override void WormCollision(Worm_Base other) {
    Base.State("Impact");
    other.State("Hurt", Base.Is("Charged") ? 3 : 1);
  }

  protected override void WormBodyCollision(WormBody_Base other) {
    Base.State("Impact");
  }

}
