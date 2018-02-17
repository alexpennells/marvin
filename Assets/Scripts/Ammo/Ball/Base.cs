using UnityEngine;
using System;
using System.Collections;

namespace AmmoBall {
  public class Base : BaseObj {
    public override void LoadReferences() {
      Physics = new Physics(Physics);
      Sound = new Sound();
      Sprite = new Sprite();
      base.LoadReferences();
    }

    public override void Step () {
      if (!HasFooting) { Physics.SkipNextFrictionUpdate(); }
      base.Step();
    }
  }
}
