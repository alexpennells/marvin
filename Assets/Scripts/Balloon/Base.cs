using UnityEngine;
using System;

namespace Balloon {
  public class Base : BaseObj {
    private bool moveUp = false;

    protected override void Init () {
      Physics.vspeed = (float)Game.Random.Next(0, 101) / 400f - 0.125f;
    }

    protected override void Step () {
      if (moveUp)
        Physics.vspeed += 0.005f;
      else
        Physics.vspeed -= 0.005f;

      if (Physics.vspeed > 0.125f)
        moveUp = false;
      else if (Physics.vspeed < -0.125f)
        moveUp = true;
    }

    public override void DestroySelf() {
      Game.CreateParticle("BalloonPop", Mask.Center);
      Sound.Play("Pop");
      base.DestroySelf();
    }
  }
}
