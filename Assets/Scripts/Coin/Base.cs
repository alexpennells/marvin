using UnityEngine;
using System;

namespace Coin {
  public class Base : BaseObj {
    private bool moveUp = false;

    public override void LoadReferences() {
      Sound = new Sound();
      base.LoadReferences();
    }

    public override void Init () {
      Physics.vspeed = (float)Game.Random.Next(0, 101) / 400f - 0.125f;
      base.Init();
    }

    public override void Step () {
      if (moveUp)
        Physics.vspeed += 0.005f;
      else
        Physics.vspeed -= 0.005f;

      if (Physics.vspeed > 0.125f)
        moveUp = false;
      else if (Physics.vspeed < -0.125f)
        moveUp = true;
      base.Step();
    }

    public override void DestroySelf() {
      Game.CreateParticle("CollectCoin", Position);
      Sound.Play("Collect");
      base.DestroySelf();
    }
  }
}
