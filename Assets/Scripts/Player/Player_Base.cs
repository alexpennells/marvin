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
  private SpriteObj chargeAura = null;

  protected override void Init() {
    ShootTimer.Interval = 800;
    TorpedoTimer.Interval = 200;
    ShieldTimer.Interval = 2000;

    chargeAura = transform.Find("ChargeAura").gameObject.GetComponent<SpriteObj>();
    ResetChargeAura();
  }

  protected override void Step () {
    if (!Is("Sliding"))
      Physics.hspeedMax = 4;

    if (Is("Torpedoing")) {
      Physics.SkipNextGravityUpdate();
      Physics.SkipNextFrictionUpdate();
    }

    base.Step();

    if (HasFooting)
      canDoubleJump = true;
  }

  private void ResetChargeAura () {
    chargeAura.SetAlpha(0f);
    chargeAura.SetSpeed(1.5f);
    chargeAura.transform.localScale = Vector3.zero;
  }

  private void GrowChargeAura () {
    float chargeSpeed = 50f;

    chargeAura.SetAlpha(Math.Min(0.8f, chargeAura.GetAlpha() + 0.8f / chargeSpeed));

    float scaleSize = chargeAura.transform.localScale.y;
    scaleSize = Math.Min(0.8f, scaleSize + 0.8f / chargeSpeed);
    chargeAura.transform.localScale = new Vector3(scaleSize, scaleSize, scaleSize);

    chargeAura.transform.localPosition = GetGunPosition();
    chargeAura.FacingLeft = Sprite.FacingLeft;
  }

  private Vector3 GetGunPosition () {
    if (Sprite.IsPlaying("idle_gun"))
      return new Vector3(14, 15, z);
    else if (Sprite.IsPlaying("walk_gun"))
      return new Vector3(16, 14, z);
    else if (Sprite.IsPlaying("torpedo"))
      return new Vector3(16, 14, z);
    else
      return new Vector3(16, 20, z);
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
    else if (Is("Ducking")) {
      ShootTimer.Enabled = false;
      ResetChargeAura();
    }
  }

  protected override void LeftHeld (float val) {
    if (Is("Ducking") || Is("Sliding") || Is("Torpedoing"))
      return;

    if (Is("Climbing")) {
        Physics.hspeed -= Physics.Climb.acceleration;
        return;
    }

    Physics.hspeed -= this.accelerationSpeed;
  }

  protected override void RightHeld (float val) {
    if (Is("Ducking") || Is("Sliding") || Is("Torpedoing"))
      return;

    if (Is("Climbing")) {
        Physics.hspeed += Physics.Climb.acceleration;
        return;
    }

    Physics.hspeed += this.accelerationSpeed;
  }

  protected override void JumpPressed () {
    if (Is("Sliding") || Is("Torpedoing"))
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
    if (Is("Sliding") || Is("Climbing") || Is("Swimming") || Is("Torpedoing"))
      return;

    if (Is("Ducking"))
      State("Slide");
    else
      State("Shoot");
  }

  protected override void AttackHeld () {
    if (!Is("Shooting"))
      return;

    // Reset timer until it's released
    ShootTimer.Enabled = false;
    ShootTimer.Enabled = true;

    GrowChargeAura();
  }

  protected override void AttackReleased () {
    if (!Is("Shooting"))
      return;

    Laser_Base laser = Game.Create("Laser", chargeAura.transform.position) as Laser_Base;
    laser.Sprite.SetAlpha(0.8f);
    laser.Sprite.StartBlur(0.01f, 0.4f, 0.1f);

    float scaleSize = chargeAura.transform.localScale.y > 0.75f ? 1f : 0.4f;
    laser.transform.localScale = new Vector3(scaleSize, scaleSize, scaleSize);
    laser.Sprite.FacingLeft = Sprite.FacingLeft;
    laser.Sprite.SetAngle(Sprite.currentRotationAngle);

    float destX = Sprite.FacingRight ? 1f : -1f;
    float destY = (float)Math.Tan(Math.PI * Sprite.currentRotationAngle / 180f) * destX;
    laser.Physics.MoveTo(new Vector2(chargeAura.transform.position.x + destX, chargeAura.transform.position.y + destY), 8f);

    ResetChargeAura();
  }

  protected override void SkatePressed () {
    if (Is("Torpedoing") || Is("Ducking") || Is("Sliding") || Is("Climbing"))
      return;

    State("Torpedo");
  }

  protected override void GrindPressed () {
    if (Is("Torpedoing") || Is("Sliding") || Is("Shielding"))
      return;

    State("Shield");
  }

  /***********************************
   * STATE CHANGE FUNCTIONS
   **********************************/

  public void StateSlide() {
    ShootTimer.Enabled = false;
    ResetChargeAura();

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
    ResetChargeAura();

    canDoubleJump = false;
    Physics.vspeed = this.jumpSpeed;
    Sprite.StartBlur(0.001f, 0.2f, 0.02f);
  }

  public void StateShoot() {
    ShootTimer.Enabled = false;
    ShootTimer.Enabled = true;
    ResetChargeAura();
  }

  public void StateClimb(Ladder_Base other) {
    ShootTimer.Enabled = false;
    ResetChargeAura();
    canDoubleJump = true;
    Sprite.StopBlur();
    Physics.Climb.Begin(other);
  }

  public void StateSwim(Water_Base other) {
    if (Physics.Swim.Begin(other)) {
      ResetChargeAura();
      ShootTimer.Enabled = false;
      canDoubleJump = true;
      Sprite.StopBlur();
    }
  }

  public void StateTorpedo() {
    ShootTimer.Enabled = false;
    ResetChargeAura();

    Sprite.Play("Torpedo");

    chargeAura.transform.localPosition = GetGunPosition();
    Torpedo_Base torpedo = Game.Create("Torpedo", chargeAura.transform.position) as Torpedo_Base;

    torpedo.Sprite.FacingLeft = Sprite.FacingLeft;
    torpedo.Sprite.SetAngle(Sprite.currentRotationAngle);

    float destX = Sprite.FacingRight ? 4f : -4f;
    float destY = (float)Math.Tan(Math.PI * Sprite.currentRotationAngle / 180f) * destX;
    torpedo.Physics.MoveTo(new Vector2(chargeAura.transform.position.x + destX, chargeAura.transform.position.y + destY), 1f);

    Physics.hspeed = -torpedo.Physics.hspeed * 2;
    Physics.vspeed = -torpedo.Physics.vspeed * 2;

    TorpedoTimer.Enabled = true;
  }

  public void StateShield() {
    GameObject p = Game.CreateParticle("ShieldAura", Mask.Center + Vector3.forward * -5f);
    p.transform.parent = transform;
    ShieldTimer.Enabled = true;
  }

  /***********************************
   * STATE CHECKERS
   **********************************/

  public bool IsSliding() { return Sprite.IsPlaying("slide"); }
  public bool IsDucking() { return Sprite.IsPlaying("duck"); }
  public bool IsShooting() { return ShootTimer.Enabled; }
  public bool IsClimbing() { return Physics.Climbing; }
  public bool IsSwimming() { return Physics.Swimming; }
  public bool IsTorpedoing() { return TorpedoTimer.Enabled; }
  public bool IsShielding() { return ShieldTimer.Enabled; }

  /***********************************
   * TIMER HANDLERS
   **********************************/

  public Timer ShootTimer { get { return Timer1; } }
  protected override void Timer1Elapsed(object source, ElapsedEventArgs e) {
    ShootTimer.Enabled = false;
  }

  public Timer TorpedoTimer { get { return Timer2; } }
  protected override void Timer2Elapsed(object source, ElapsedEventArgs e) {
    TorpedoTimer.Enabled = false;
  }

  public Timer ShieldTimer { get { return Timer3; } }
  protected override void Timer3Elapsed(object source, ElapsedEventArgs e) {
    ShieldTimer.Enabled = false;
  }
}
