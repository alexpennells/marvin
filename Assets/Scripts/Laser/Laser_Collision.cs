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

}
