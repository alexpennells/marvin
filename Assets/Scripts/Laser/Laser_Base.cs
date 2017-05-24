using UnityEngine;
using System;

public class Laser_Base : BaseObj {
  private bool submerged = false;
  private Vector2 startSpeed;

  protected override void OffScreenStep() {
    DestroySelf();
  }

  /***********************************
   * STATE CHANGE FUNCTIONS
   **********************************/

  public void StateImpact() {
    Physics.hspeed = 0;
    Physics.vspeed = 0;
    Game.CreateParticle(Is("Charged") ? "LaserCollideLarge" : "LaserCollideSmall", Mask.Center);
    DestroySelf();
  }

  public void StateSubmerged() {
    this.startSpeed = new Vector2(Physics.hspeed, Physics.vspeed);
    Physics.hspeed = Physics.hspeed / 3f;
    Physics.vspeed = Physics.vspeed / 3f;
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

  public bool IsSubmerged() { return submerged; }
  public bool IsCharged() { return transform.localScale.y > 0.9f; }
}
