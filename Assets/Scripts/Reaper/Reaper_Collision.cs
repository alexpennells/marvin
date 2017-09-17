using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Reaper_Collision : CollisionStubs {
  public override eObjectType Type { get { return eObjectType.REAPER; } }
}
