using UnityEngine;
using System;

public class Bullet_Base : BaseObj {
  protected override void OffScreenStep() {
    DestroySelf();
  }

  /***********************************
   * STATE CHANGE FUNCTIONS
   **********************************/

  public void StateImpact() {
    Physics.hspeed = 0;
    Physics.vspeed = 0;
    Game.CreateParticle("BulletCollide", Mask.Center);
    DestroySelf();
  }
}
