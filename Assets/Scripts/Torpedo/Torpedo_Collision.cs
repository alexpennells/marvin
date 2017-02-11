using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Torpedo_Collision : CollisionStubs
{
  public override eObjectType Type { get { return eObjectType.TORPEDO; } }
}
