using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Bullet_Collision : CollisionStubs
{
  public override eObjectType Type { get { return eObjectType.BULLET; } }

  // protected override void WormCollision(Worm_Base other) {
  //   Base.State("Impact");
  // }

  // protected override void WormBodyCollision(WormBody_Base other) {
  //   Base.State("Impact");
  // }

}
