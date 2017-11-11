using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Target_Collision : CollisionStubs
{
  public override eObjectType Type { get { return eObjectType.TARGET; } }

  protected override void BulletCollision(Bullet_Base other) {
    if (other.impacted)
      return;

    other.impacted = true;
    if (Base.Is("Inhaling") || Base.Is("Stretching")) {
      Base.State("Hurt");
    } else {
      Base.State("Break");
    }
  }
}
