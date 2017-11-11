using UnityEngine;
using System;
using System.Timers;

public class Bullet_Base : BaseObj {
  public bool impacted = false;

  protected override void Init() {
    PopTimer.Interval = 300;
    PopTimer.Enabled = true;
    Sound.Play("Shoot");
  }

  protected override void Step() {
    if (impacted)
      State("Impact");
  }

  /***********************************
   * STATE CHANGE FUNCTIONS
   **********************************/

  public void StateImpact() {
    Physics.hspeed = 0;
    Physics.vspeed = 0;
    Game.CreateParticle("BulletCollide", Mask.Center);
    Sound.Play("Pop");
    DestroySelf();
  }

  public Timer PopTimer { get { return Timer1; } }
  protected override void Timer1Elapsed(object source, ElapsedEventArgs e) {
    PopTimer.Enabled = false;
    impacted = true;
  }
}
