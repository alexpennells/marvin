using UnityEngine;
using System;
using System.Timers;

public class Torpedo_Base : BaseObj {
  private bool blurStarted = false;

  protected override void Init() {
    BoostTimer.Interval = 500;
    BoostTimer.Enabled = true;
  }

  protected override void Step() {
    Game.CreateParticle("TorpedoPuff", Mask.Center);

    if (!BoostTimer.Enabled && !this.blurStarted) {
      Sprite.StartBlur(0.01f, 0.5f, 0.1f);
      this.blurStarted = true;
    }
  }

  protected override void OffScreenStep() {
    DestroySelf();
  }

  /***********************************
   * STATE CHANGE FUNCTIONS
   **********************************/

  public void StateImpact() {
    Physics.hspeed = 0;
    Physics.vspeed = 0;
    Game.CreateParticle("TorpedoExplosion", Mask.Center);
    DestroySelf();
  }

  /***********************************
   * TIMER HANDLERS
   **********************************/

  public Timer BoostTimer { get { return Timer1; } }
  protected override void Timer1Elapsed(object source, ElapsedEventArgs e) {
    BoostTimer.Enabled = false;
    Physics.hspeed = Physics.hspeed * 5;
    Physics.vspeed = Physics.vspeed * 5;
  }

}
