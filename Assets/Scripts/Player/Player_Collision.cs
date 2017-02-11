using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player_Collision : CollisionStubs
{
  public override eObjectType Type { get { return eObjectType.PLAYER; } }

  protected override void LadderCollision(Ladder_Base other) {
    if (!Base.Is("Climbing") && !Base.Is("Torpedoing") && (Game.UpHeld || Game.DownHeld))
      Base.State("Climb", other);
  }

  protected override void LadderExit(Ladder_Base other) {
    if (Base.Is("Climbing") && Base.Physics.Climb.Ladder == other)
      Base.Physics.Climb.Stop();
  }

  protected override void WaterCollision(Water_Base other) {
    if (!Base.Is("Swimming"))
      Base.State("Swim", other);
  }

  protected override void WaterExit(Water_Base other) {
    if (Base.Is("Swimming") && Base.Physics.Swim.Water == other)
      Base.Physics.Swim.Stop();
  }

}
