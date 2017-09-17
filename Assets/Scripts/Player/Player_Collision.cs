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

  protected override void WormCollision(Worm_Base other) {
    Base.State("Hurt", other.x >= Base.x, 50);
  }

  protected override void WormBodyCollision(WormBody_Base other) {
    Base.State("Hurt", other.x >= Base.x, 50);
  }
}
