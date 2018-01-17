using UnityEngine;
using System;
using System.Collections;

namespace Player {
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
    private bool dead = false;
    private BaseObj hand = null;

    protected override void LoadReferences() {
      Sprite = new Sprite();
      Sprite.enabled = true;
      base.LoadReferences();
    }

    protected override void Step () {
      // Destroy old hands that are no longer being used.
      if (!Is("Dead") && !HasFooting && this.hand != null) {
        this.hand.DestroySelf();
        this.hand = null;
      }

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

      if (Is("Dead")) {
        Physics.SkipNextFrictionUpdate();
        Physics.SkipNextGravityUpdate();
        Physics.hspeed = 0;
        Physics.vspeed = 0;
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
      if (Is("Dead") || Is("Ziplining"))
        return;

      if (Is("Climbing")) {
        Physics.hspeed -= Physics.Climb.acceleration;
        return;
      }

      if (SolidPhysics.WallJump.IsJumping())
        return;

      // Don't increase speed if walk speed is maxed out.
      if (!Is("Running") && Physics.hspeed <= -maxWalkingHspeed)
        return;

      Physics.hspeed -= TrueAccelerationSpeed;
    }

    protected override void RightHeld (float val) {
      if (Is("Dead") || Is("Ziplining"))
        return;

      if (Is("Climbing")) {
        Physics.hspeed += Physics.Climb.acceleration;
        return;
      }

      if (SolidPhysics.WallJump.IsJumping())
        return;

      // Don't increase speed if walk speed is maxed out.
      if (!Is("Running") && Physics.hspeed >= maxWalkingHspeed)
        return;

      Physics.hspeed += TrueAccelerationSpeed;
    }

    protected override void UnoPressed () {
      RestartCoroutine("JumpPress");

      if (Is("Dead"))
        return;

      if (Is("Ziplining")) {
        Physics.Rail.StopGrinding();
        Physics.vspeed = this.jumpSpeed;
      } else if (Is("WallSliding")) {
        Sound.Play("Jump");
        SolidPhysics.WallJump.ActuallyWallJump();
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

      if (!SolidPhysics.WallJump.IsJumping() && Physics.vspeed > 2)
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
      if (Is("Invincible") || Is("Dead"))
        return;

      Physics.hspeed = 0;
      Physics.vspeed = 0;

      if (!HasFooting)
        this.hand = Game.Create("Hand", Position);

      Game.CreateParticle("Blood", Mask.Center);
      BaseObj ghost = Game.Create("PlayerGhost", Position);
      ghost.Physics.hspeed = moveLeft ? -2.5f : 2.5f;
      ghost.Physics.vspeed = 2;

      Sprite.Play("Hurt");
      Game.Camera.LoadFocus();
      dead = true;
    }

    public void StateRevive() {
      Game.CreateParticle("PlayerRevive", Mask.Center);
      Sprite.Play("Idle");
      dead = false;
      StartCoroutine("Invincible");
    }

    public void StateBounce() {
      Physics.vspeed = this.jumpPress ? 5 : 3;
      Sound.Play("Thump");
      Sprite.Play("Spin");
    }

    public void StateExit(BaseObj exit) {
      Game.disableInput = true;
      StartCoroutine(WalkToPoint(exit.Position, "Exit", exit));
    }

    /***********************************
     * STATE CHECKERS
     **********************************/

    public bool IsRunning() { return Game.RightTriggerHeld; }
    public bool IsClimbing() { return Physics.Climbing; }
    public bool IsWallSliding() { return SolidPhysics.WallJump.Sliding; }
    public bool IsInvincible() { return invincible; }
    public bool IsZiplining() { return Physics.Grinding; }
    public bool IsDead() { return dead; }

    /***********************************
     * CO-ROUTINES
     **********************************/

    private IEnumerator WalkToPoint(Vector3 pos, string callback, BaseObj arg) {
      Physics.hspeed = 0;

      if (x > pos.x)
        Sprite.FacingLeft = true;
      else if (x < pos.x)
        Sprite.FacingRight = true;
      Sprite.Play("Walk");

      float moveSpeed = 0.0075f;
      while (x != pos.x) {
        if (Sprite.FacingLeft) {
          if ((x - pos.x) > moveSpeed)
            x -= moveSpeed;
          else {
            x = pos.x;
            break;
          }
        } else {
          if ((pos.x - x) > moveSpeed)
            x += moveSpeed;
          else {
            x = pos.x;
            break;
          }
        }

        yield return null;
      }

      Callback(callback, arg);
    }

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

    /***********************************
     * CO-ROUTINE CALLBACKS
     **********************************/

    public void CallbackExit(BaseObj exit) {
      exit.State("TransitionStart");
    }
  }
}
