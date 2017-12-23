using UnityEngine;
using System;
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

    private bool jumpPress = false;
    private bool invincible = false;
    private bool hurt = false;
    private bool dead = false;

    protected override void Step () {
      if (Is("WallSliding")) {
        Sound.StartLoop("Slide");
      } else if (HasFooting && Physics.hspeed != 0 && !Game.LeftHeld && !Game.RightHeld) {
        Sound.StartLoop("Slide");
        CreateWalkPuffs(1);
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

      if (Game.LeftHeld || Game.RightHeld || !HasFooting)
        Physics.SkipNextFrictionUpdate();

      // Cap the max walk speed.
      if (!Is("Running") || Is("Ziplining")) {
        if (Physics.hspeed > maxWalkingHspeed)
          Physics.hspeed = Math.Max(Physics.hspeed - 0.1f, -maxWalkingHspeed);
        else if (Physics.hspeed < -maxWalkingHspeed)
          Physics.hspeed = Math.Min(Physics.hspeed + 0.1f, -maxWalkingHspeed);
      }

      base.Step();
    }

    public void CreateWalkPuffs(int count) {
      for (int i = 0; i < count; i++)
        Game.CreateParticle("Puff", Position);
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
      if (Is("Hurt") || Is("Dead") || Is("Ziplining"))
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
      if (Is("Hurt") || Is("Dead") || Is("Ziplining"))
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
      RestartCoroutine("JumpPress");

      if (Is("Hurt") || Is("Dead"))
        return;

      if (Is("Ziplining")) {
        Physics.Rail.StopGrinding();
        Physics.vspeed = this.jumpSpeed;
      } else if (Is("WallSliding")) {
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

    public void StateHurt(bool moveLeft) {
      if (Is("Hurt") || Is("Invincible") || Is("Dead"))
        return;

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
        StartCoroutine("Invincible");
        StartCoroutine("Hurt");
      // }
    }

    public void StateDie() {
      Sprite.Play("Die");
      Physics.vspeed = 2;
      StartCoroutine("Dead");
    }

    public void StateBounce() {
      Physics.vspeed = this.jumpPress ? 5 : 3;
    }

    /***********************************
     * STATE CHECKERS
     **********************************/

    public bool IsRunning() { return Game.RightTriggerHeld; }
    public bool IsClimbing() { return Physics.Climbing; }
    public bool IsWallSliding() { return SolidPhysics.Walljump.Sliding; }
    public bool IsHurt() { return hurt; }
    public bool IsInvincible() { return invincible; }
    public bool IsZiplining() { return Physics.Grinding; }
    public bool IsDead() { return dead; }

    /***********************************
     * CO-ROUTINES
     **********************************/

    private IEnumerator JumpPress() {
      jumpPress = true;
      yield return new WaitForSeconds(0.25f);
      jumpPress = false;
    }

    private IEnumerator Invincible() {
      invincible = true;
      yield return new WaitForSeconds(0.8f);
      invincible = false;
    }

    private IEnumerator Hurt() {
      hurt = true;
      yield return new WaitForSeconds(0.25f);
      hurt = false;
    }

    private IEnumerator Dead() {
      dead = true;
      yield return new WaitForSeconds(1);
      Game.ChangeScene("MainHub", 0, "CatHead");
    }
  }
}
