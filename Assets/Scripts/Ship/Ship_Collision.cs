using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Ship_Collision : CollisionStubs {
  public override eObjectType Type { get { return eObjectType.SHIP; } }
}
