using UnityEngine;
using System;

namespace Balloon {
  public class Base : BaseObj {
    private bool moveUp = false;

    public override void LoadReferences() {
      Sprite = new Sprite();
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
      for (int i = 0; i < 5; i++)
        Game.Create("Soul", Mask.Center);

      Game.CreateParticle("BalloonPop", Mask.Center);
      Sound.Play("Pop");
      base.DestroySelf();
    }
  }
}
