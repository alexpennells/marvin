using UnityEngine;
using System;
using System.Collections;

namespace Player {
  public class Base : BaseObj {

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
    private bool throwing = false;
    private bool dead = false;
    private bool headless = false;

    private bool pound = false;
    public bool Pound { get { return pound; } set { pound = value; } }

    public float crosshairAngle = 0;
    private float crosshairRadius = 1f;
    private SpriteRenderer crosshair;
    private float crosshairSpeed = 0.05f;

    private SpriteRenderer dot1;
    private SpriteRenderer dot2;
    private SpriteRenderer dot3;

    public override void LoadReferences() {
      Sprite = new Sprite();
      Sound = new Sound();
      Physics = new Physics(Physics);
      Input = new Input();

      crosshair = transform.Find("Crosshair").GetComponent<SpriteRenderer>();
      dot1 = transform.Find("Dot1").GetComponent<SpriteRenderer>();
      dot2 = transform.Find("Dot2").GetComponent<SpriteRenderer>();
      dot3 = transform.Find("Dot3").GetComponent<SpriteRenderer>();
      base.LoadReferences();
    }

    public override void Step() {
      UpdateCrosshair();

      // Adjust mask height based on whether the player is ducking
      Mask.top = Is("Ducking") ? 0.16f : 0.24f;

      if (Is("Throwing") && Sprite.speed == 0) {
        crosshairAngle += crosshairSpeed;

        if (crosshairSpeed > 0 && crosshairAngle >= Math.PI / 2)
          crosshairSpeed = -crosshairSpeed;
        else if (crosshairSpeed < 0 && crosshairAngle <= -Math.PI / 2)
          crosshairSpeed = -crosshairSpeed;
      }

      if (Is("WallSliding")) {
        Sound.StartLoop("Slide");
      } else if (HasFooting && Physics.hspeed != 0 && (!Game.DirectionHeld || Is("Throwing") || Is("Ducking"))) {
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

      if (!HasFooting || (Game.DirectionHeld && !Is("Throwing") && !Is("Dead") && !Is("Ducking")))
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

    public void ThrowBullet() {
      if (Stitch.Ammo.Count > 0) {
        Game.Create("Bullets/" + Stitch.GetAmmoType(Stitch.Ammo[0]), Position + new Vector3(-0.1f, 0.117f, 0));
        Stitch.PopAmmo();
      }
    }

    public void EndThrow() {
      throwing = false;
      Sprite.Play("Idle");
    }

    public Vector3 AmmoPosition() {
      if (!Is("Throwing"))
        return Vector3.zero;

      if (Sprite.GetAnimationTime() <= 0.25f) {
        return new Vector3(-0.12f, 0.053f, 0);
      } else if (Sprite.GetAnimationTime() <= 0.5f) {
        return new Vector3(-0.1f, 0.177f, 0);
      } else {
        return Vector3.zero;
      }
    }

    private void UpdateCrosshair() {
      if (Is("Throwing") && crosshair.color.a < 1f)
        crosshair.color = new Color(1, 1, 1, Math.Min(crosshair.color.a + 0.05f, 1f));
      else if (!Is("Throwing") && crosshair.color.a != 0)
        crosshair.color = new Color(1, 1, 1, 0);

      crosshair.transform.position = new Vector3(
        Mask.Center.x + crosshairRadius * (float)Math.Cos(crosshairAngle),
        Mask.Center.y + crosshairRadius * (float)Math.Sin(crosshairAngle)
      );
      if (Sprite.FacingLeft) {
        float dist = crosshair.transform.position.x - x;
        crosshair.transform.position = new Vector3(crosshair.transform.position.x - dist * 2, crosshair.transform.position.y, crosshair.transform.position.z);
      }

      float radius = crosshairRadius / 4f;
      dot1.color = crosshair.color;
      dot1.transform.position = new Vector3(
        Mask.Center.x + radius * (float)Math.Cos(crosshairAngle),
        Mask.Center.y + radius * (float)Math.Sin(crosshairAngle)
      );
      if (Sprite.FacingLeft) {
        float dist = dot1.transform.position.x - x;
        dot1.transform.position = new Vector3(dot1.transform.position.x - dist * 2, dot1.transform.position.y, dot1.transform.position.z);
      }

      radius *= 2;
      dot2.color = crosshair.color;
      dot2.transform.position = new Vector3(
        Mask.Center.x + radius * (float)Math.Cos(crosshairAngle),
        Mask.Center.y + radius * (float)Math.Sin(crosshairAngle)
      );
      if (Sprite.FacingLeft) {
        float dist = dot2.transform.position.x - x;
        dot2.transform.position = new Vector3(dot2.transform.position.x - dist * 2, dot2.transform.position.y, dot2.transform.position.z);
      }

      radius = radius + crosshairRadius / 4f;
      dot3.color = crosshair.color;
      dot3.transform.position = new Vector3(
        Mask.Center.x + radius * (float)Math.Cos(crosshairAngle),
        Mask.Center.y + radius * (float)Math.Sin(crosshairAngle)
      );
      if (Sprite.FacingLeft) {
        float dist = dot3.transform.position.x - x;
        dot3.transform.position = new Vector3(dot3.transform.position.x - dist * 2, dot3.transform.position.y, dot3.transform.position.z);
      }
    }

    public void WaitForRevive() {
      StartCoroutine("Revive");
      Sprite.SetSpeed(0);
    }

    /***********************************
     * STATE CHANGE FUNCTIONS
     **********************************/

    public void StateHurt(bool moveLeft) {
      if (Is("Invincible") || Is("Dead"))
        return;

      Physics.hspeed = moveLeft ? -3 : 3;
      Physics.vspeed = 2;
      Pound = false;
      Sprite.FacingLeft = !moveLeft;

      if (!headless) {
        BaseObj head = Game.Create("Ammo/Head", Mask.Center);
        head.Physics.vspeed = 3;
        head.Physics.hspeed = moveLeft ? -3.1f : 3.1f;
        head.Sprite.FacingLeft = Sprite.FacingLeft;

        BaseObj ghost = Game.Create("PlayerGhost", Mask.Center);
        ghost.Physics.vspeed = 1;
        ghost.Physics.hspeed = moveLeft ? 3 : -3;
      }
      headless = true;

      Sprite.Play("Fall");
      Game.CreateParticle("Blood", Mask.Center);
      dead = true;
    }

    public void StatePound() {
      if (Is("WallSliding") || Is("Ziplining") || Is("Dead") || Is("Throwing") || Is("Pounding"))
        return;

      Physics.hspeed = 0;
      Physics.vspeed = 0;
      StartCoroutine("GroundPoundSpin");
      Sound.Play("Pound");
      pound = true;
    }

    public void StateRevive() {
      dead = false;
      Sprite.Play("Idle");
      StartCoroutine("Invincible");
    }

    public void StateBounce() {
      Pound = false;
      Physics.vspeed = this.jumpPress ? 5 : 3;
      Sound.Play("Thump");
      Sprite.Play("Spin");
    }

    public void StateThrow() {
      if (Is("WallSliding") || Is("Ziplining") || Is("Dead") || Is("Throwing"))
        return;

      if (Game.UpHeld) {
        crosshairAngle = (float)Math.PI / 2;
        crosshairSpeed = -Math.Abs(crosshairSpeed);
      } else {
        crosshairAngle = -(float)Math.PI / 4;
        crosshairSpeed = Math.Abs(crosshairSpeed);
      }

      Sprite.Play("Throw");
      throwing = true;
    }

    public void StateCancelThrow() {
      if (!Is("Throwing"))
        return;

      Stitch.Instance.CycleAmmo();
      throwing = false;
    }

    public void StateExit(BaseObj exit) {
      Game.disableInput = true;
      StartCoroutine(WalkToPoint(exit.Position, "Exit", exit));
    }

    public void StateReviveHead() {
      Game.CreateParticle("PlayerRevive", Mask.Center);
      Sprite.scaleY = 0.25f;
      headless = false;
    }

    /***********************************
     * STATE CHECKERS
     **********************************/

    public bool IsRunning() { return Game.RightTriggerHeld; }
    public bool IsDucking() { return HasFooting && Game.DownHeld; }
    public bool IsPounding() { return pound; }
    public bool IsWallSliding() { return Physics.Ground.WallJump.Sliding; }
    public bool IsInvincible() { return invincible; }
    public bool IsThrowing() { return throwing; }
    public bool IsZiplining() { return Physics.Grinding; }
    public bool IsDead() { return dead; }
    public bool IsHeadless() { return headless; }

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

    private IEnumerator Revive() {
      yield return new WaitForSeconds(0.75f);
      Sprite.Play("Revive");
    }

    private IEnumerator GroundPoundSpin() {
      Vector3 origin = Mask.Center;
      Physics.Ground.enabled = false;

      for (int i = 0; i < 360; i += 24) {
        transform.RotateAround(origin, Vector3.forward, 24);
        yield return null;
      }

      Sprite.SetAngle(0);
      Physics.Ground.enabled = true;
      yield return new WaitForSeconds(0.1f);

      Physics.vspeed = -Physics.vspeedMax;
    }

    private IEnumerator GroundPound() {
      Sprite.scaleY = 0.5f;
      Game.Camera.StartCoroutine("Shake");
      Sound.Play("GroundPound");
      Game.CreateParticle("GroundPound", Position);
      yield return new WaitForSeconds(0.1f);
      Pound = false;
    }

    /***********************************
     * CO-ROUTINE CALLBACKS
     **********************************/

    public void CallbackExit(BaseObj exit) {
      exit.State("TransitionStart");
    }
  }
}
