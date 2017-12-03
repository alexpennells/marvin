using UnityEngine;
using System;
using System.Timers;
using System.Collections;

namespace Player {
  [RequireComponent (typeof (Sprite))]
  [RequireComponent (typeof (Sound))]
  [RequireComponent (typeof (SolidCollider))]
  public class Base : InputObj {

    [Tooltip("Vertical speed of the player when jumping")]
    public float jumpSpeed = 6;

    [Tooltip("The amount of speed the player picks up per step")]
    public float accelerationSpeed = 0.3f;
    public float TrueAccelerationSpeed {
      get {
        if (!Is("Running") && HasFooting)
          return this.accelerationSpeed / 1.5f;
        else
          return this.accelerationSpeed;
      }
    }

    [Tooltip("This max hspeed when walking")]
    public float maxWalkingHspeed = 3f;

    private bool deadTimerExpired = false;

    protected override void Init() {
      HurtTimer.Interval = 250;
      InvincibleTimer.Interval = 800;
      DeadTimer.Interval = 1000;
    }

    protected override void Step () {
      if (deadTimerExpired) {
        deadTimerExpired = false;
        Game.ChangeScene("MainHub", 0, "CatHead");
      }

      if (Is("WallSliding")) {
        Sound.StartLoop("Slide");
      } else if (HasFooting && Physics.hspeed != 0 && !Game.LeftHeld && !Game.RightHeld) {
        Sound.StartLoop("Slide");
      } else {
        Sound.StopLoop("Slide");
      }

      // Adjust max speed based on running
      if (!Is("Running") && Math.Abs(this.Physics.hspeed) > 3f) {
        if (this.Physics.hspeed > 0)
          this.Physics.hspeed = Math.Max(this.Physics.hspeed, this.Physics.hspeed - 0.05f);
        else
          this.Physics.hspeed = Math.Min(this.Physics.hspeed, this.Physics.hspeed + 0.05f);
      }

      base.Step();
    }

    public void StartDeadTimer() {
      DeadTimer.Enabled = true;
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
      if (Is("Hurt") || Is("Dead"))
        return;

      if (Is("Climbing")) {
        Physics.hspeed -= Physics.Climb.acceleration;
        return;
      }

      if (SolidPhysics.Walljump.IsJumping())
        return;

      // Don't increase speed if walk speed is maxed out.
      if (!Is("Running") && Physics.hspeed <= -maxWalkingHspeed)
        return;

      Physics.hspeed -= TrueAccelerationSpeed;
    }

    protected override void RightHeld (float val) {
      if (Is("Hurt") || Is("Dead"))
        return;

      if (Is("Climbing")) {
        Physics.hspeed += Physics.Climb.acceleration;
        return;
      }

      if (SolidPhysics.Walljump.IsJumping())
        return;

      // Don't increase speed if walk speed is maxed out.
      if (!Is("Running") && Physics.hspeed >= maxWalkingHspeed)
        return;

      Physics.hspeed += TrueAccelerationSpeed;
    }

    protected override void UnoPressed () {
      if (Is("Hurt") || Is("Dead"))
        return;

      if (Is("WallSliding")) {
        Sound.Play("Jump");
        SolidPhysics.Walljump.ActuallyWalljump();
      } else if (HasFooting || Is("Climbing")) {
        Physics.vspeed = this.jumpSpeed;
        Sound.Play("Jump");
        SolidPhysics.Collider.ClearFooting();
        Physics.Climb.Stop();
      }
    }

    protected override void UnoReleased () {
      if (Is("Dead"))
        return;

      if (!SolidPhysics.Walljump.IsJumping() && Physics.vspeed > 2)
        Physics.vspeed = 2;
    }

    protected override void DosPressed () {
    }

    protected override void DosHeld () {
    }

    protected override void DosReleased () {
    }

    protected override void TresPressed () {
    }

    protected override void CuatroPressed () {
    }

    protected override void LeftTriggerHeld(float val) {
    }

    protected override void RightTriggerHeld(float val) {

    }

    /***********************************
     * STATE CHANGE FUNCTIONS
     **********************************/

    public void StateClimb(Ladder_Base other) {
      Sprite.Play("Climb");
      Sprite.StopBlur();
      Physics.Climb.Begin(other);
    }

    public void StateHurt(bool moveLeft, int damage) {
      if (Is("Hurt") || Is("Invincible") || Is("Dead"))
        return;

      // Game.HUD.ReduceShield(damage);
      Sprite.FacingLeft = !moveLeft;

      Physics.vspeed = 0;

      if (Sprite.FacingLeft)
        Physics.hspeed = 2;
      else if (Sprite.FacingRight)
        Physics.hspeed = -2;

      Game.CreateParticle("Blood", Mask.Center);

      // if (Game.HUD.IsDead()) {
        // State("Die");
      // } else {
        Sprite.Play("Hurt");
        HurtTimer.Enabled = true;
        InvincibleTimer.Enabled = true;
      // }
    }

    public void StateDie() {
      Sprite.Play("Die");
      Physics.vspeed = 2;
    }

    /***********************************
     * STATE CHECKERS
     **********************************/

    public bool IsRunning() { return Game.RightTriggerHeld; }
    public bool IsClimbing() { return Physics.Climbing; }
    public bool IsWallSliding() { return SolidPhysics.Walljump.Sliding; }
    public bool IsHurt() { return HurtTimer.Enabled; }
    public bool IsInvincible() { return InvincibleTimer.Enabled; }
    public bool IsDead() { return Sprite.IsPlaying("die_fall", "die_land"); }

    /***********************************
     * TIMER HANDLERS
     **********************************/

    public Timer DeadTimer { get { return Timer3; } }
    protected override void Timer3Elapsed(object source, ElapsedEventArgs e) {
      deadTimerExpired = true;
      DeadTimer.Enabled = false;
    }

    public Timer HurtTimer { get { return Timer5; } }
    protected override void Timer5Elapsed(object source, ElapsedEventArgs e) {
      HurtTimer.Enabled = false;
    }

    public Timer InvincibleTimer { get { return Timer6; } }
    protected override void Timer6Elapsed(object source, ElapsedEventArgs e) {
      InvincibleTimer.Enabled = false;
    }
  }
}
