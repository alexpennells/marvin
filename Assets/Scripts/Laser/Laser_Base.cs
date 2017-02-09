using UnityEngine;
using System;

public class Laser_Base : BaseObj {
  private bool impacted = false;
  private bool submerged = false;
  private Vector2 startSpeed;

  protected override void Step() {
    if (Is("Impacted")) {
      Sprite.SetAlpha(Sprite.GetAlpha() - 0.1f);

      if (Sprite.GetAlpha() <= 0f)
        DestroySelf();
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
    impacted = true;
    Sprite.Play("Die");
  }

  public void StateSubmerged() {
    this.startSpeed = new Vector2(Physics.hspeed, Physics.vspeed);
    Physics.hspeed = Physics.hspeed / 2f;
    Physics.vspeed = Physics.vspeed / 2f;
    this.submerged = true;
  }

  public void StateAirborne() {
    Physics.hspeed = this.startSpeed.x;
    Physics.vspeed = this.startSpeed.y;
    this.submerged = false;
  }

  /***********************************
   * STATE CHECKERS
   **********************************/

  public bool IsImpacted() { return impacted; }
  public bool IsSubmerged() { return submerged; }
}
