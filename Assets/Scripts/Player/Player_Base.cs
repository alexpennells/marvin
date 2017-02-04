using UnityEngine;
using System;
using System.Timers;
using System.Collections;

public class Player_Base : InputObj {

  [Tooltip("Vertical speed of the player when jumping")]
  public float jumpSpeed = 6;

  [Tooltip("The amount of speed the player picks up per step")]
  public float accelerationSpeed = 0.3f;

  private bool canDoubleJump = true;

  protected override void Init() {
    ShootTimer.Interval = 2000;
  }

  protected override void Step () {
    if (!Is("Sliding"))
      Physics.hspeedMax = 4;

    base.Step();

    if (HasFooting)
      canDoubleJump = true;
  }

  /***********************************
   * INPUT HANDLERS
   **********************************/

  protected override void UpHeld(float val) {
    if (Is("Climbing"))
      Physics.vspeed += Physics.Climb.acceleration;
  }

  protected override void DownHeld(float val) {
    if (Is("Climbing"))
      Physics.vspeed -= Physics.Climb.acceleration;
  }

  protected override void LeftHeld (float val) {
    if (Is("Ducking") || Is("Sliding"))
      return;

    if (Is("Climbing")) {
        Physics.hspeed -= Physics.Climb.acceleration;
        return;
    }

    Physics.hspeed -= this.accelerationSpeed;
  }

  protected override void RightHeld (float val) {
    if (Is("Ducking") || Is("Sliding"))
      return;

    if (Is("Climbing")) {
        Physics.hspeed += Physics.Climb.acceleration;
        return;
    }

    Physics.hspeed += this.accelerationSpeed;
  }

  protected override void JumpPressed () {
    if (Is("Sliding"))
      return;

    if (Is("Swimming")) {
      Physics.Swim.Stroke();
      SolidPhysics.Collider.ClearFooting();
    } else if (HasFooting || Is("Climbing")) {
      Physics.vspeed = this.jumpSpeed;
      SolidPhysics.Collider.ClearFooting();
      Physics.Climb.Stop();
    } else if (canDoubleJump)
      State("DoubleJump");
  }

  protected override void JumpReleased () {
    if (Physics.vspeed > 2)
      Physics.vspeed = 2;
  }

  protected override void AttackPressed () {
    if (Is("Sliding") || Is("Climbing") || Is("Swimming"))
      return;

    if (Is("Ducking"))
      State("Slide");
    else
      State("Shoot");
  }

  /***********************************
   * STATE CHANGE FUNCTIONS
   **********************************/

  public void StateSlide() {
    ShootTimer.Enabled = false;

    Sprite.Play("Slide");
    Sprite.StartBlur(0.001f, 0.2f, 0.02f);
    Physics.hspeedMax = 5;

    if (Sprite.FacingLeft)
      Physics.hspeed = -Physics.hspeedMax;
    else if (Sprite.FacingRight)
      Physics.hspeed = Physics.hspeedMax;
  }

  public void StateDoubleJump() {
    ShootTimer.Enabled = false;

    canDoubleJump = false;
    Physics.vspeed = this.jumpSpeed;
    Sprite.StartBlur(0.001f, 0.2f, 0.02f);
  }

  public void StateShoot() {
    ShootTimer.Enabled = false;
    ShootTimer.Enabled = true;
  }

  public void StateClimb(Ladder_Base other) {
    ShootTimer.Enabled = false;
    canDoubleJump = true;
    Sprite.StopBlur();
    Physics.Climb.Begin(other);
  }

  public void StateSwim(Water_Base other) {
    if (Physics.Swim.Begin(other)) {
      ShootTimer.Enabled = false;
      canDoubleJump = true;
      Sprite.StopBlur();
    }
  }

  /***********************************
   * STATE CHECKERS
   **********************************/

  public bool IsSliding() { return Sprite.IsPlaying("slide"); }
  public bool IsDucking() { return Sprite.IsPlaying("duck"); }
  public bool IsShooting() { return ShootTimer.Enabled; }
  public bool IsClimbing() { return Physics.Climbing; }
  public bool IsSwimming() { return Physics.Swimming; }

  /***********************************
   * TIMER HANDLERS
   **********************************/

  public Timer ShootTimer { get { return Timer1; } }
  protected override void Timer1Elapsed(object source, ElapsedEventArgs e) {
    ShootTimer.Enabled = false;
  }

}
