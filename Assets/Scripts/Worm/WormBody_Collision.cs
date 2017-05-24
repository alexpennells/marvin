using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WormBody_Collision : CollisionStubs {
  public override eObjectType Type { get { return eObjectType.WORM_BODY; } }
}
