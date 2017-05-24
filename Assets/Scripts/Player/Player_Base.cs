using UnityEngine;
using System;
using System.Timers;
using System.Collections;

public class Player_Base : InputObj {

  [Tooltip("Vertical speed of the player when jumping")]
  public float jumpSpeed = 6;

  [Tooltip("Vertical speed of the player when double jumping")]
  public float doubleJumpSpeed = 5;

  [Tooltip("The amount of speed the player picks up per step")]
  public float accelerationSpeed = 0.3f;

  private bool canDoubleJump = true;
  public bool CanDoubleJump { get { return canDoubleJump; } set { canDoubleJump = value; } }

  private bool facingLeftBeforeRoll = false;
  public bool FacingLeftBeforeRoll { get { return facingLeftBeforeRoll; } }

  private bool flying = false;
  private SpriteObj chargeAura = null;
  private GameObject chargeLight = null;

  protected override void Init() {
    ShootTimer.Interval = 800;
    TorpedoTimer.Interval = 200;
    RollTimer.Interval = 25;
    HurtTimer.Interval = 250;
    InvincibleTimer.Interval = 800;

    chargeAura = transform.Find("ChargeAura").gameObject.GetComponent<SpriteObj>();
    chargeLight = chargeAura.transform.Find("Light").gameObject;
    ResetChargeAura();
  }

  protected override void Step () {
    if (Is("Rolling"))
      Physics.SkipNextFrictionUpdate();
    else
      Physics.hspeedMax = 5;

    if (Is("Torpedoing")) {
      Physics.SkipNextGravityUpdate();
      Physics.SkipNextFrictionUpdate();
    }

    if (Is("Flying")) {
      Physics.SkipNextGravityUpdate();
      Physics.applyFrictionToVspeed = true;
    } else
      Physics.applyFrictionToVspeed = false;

    base.Step();

    if (HasFooting)
      canDoubleJump = true;
  }

  private void ResetChargeAura () {
    chargeAura.SetAlpha(0f);
    chargeAura.SetSpeed(1.5f);
    chargeAura.transform.localScale = Vector3.zero;

    chargeLight.SetActive(false);
  }

  private void GrowChargeAura () {
    float chargeSpeed = 50f;

    chargeAura.SetAlpha(Math.Min(1f, chargeAura.GetAlpha() + 1f / chargeSpeed));

    float scaleSize = chargeAura.transform.localScale.y;
    scaleSize = Math.Min(0.8f, scaleSize + 0.8f / chargeSpeed);
    chargeAura.transform.localScale = new Vector3(scaleSize, scaleSize, scaleSize);

    chargeAura.transform.localPosition = GetGunPosition();
    chargeAura.FacingLeft = Sprite.FacingLeft;

    chargeLight.SetActive(true);
    chargeLight.GetComponent<Light>().intensity = scaleSize * 3;
  }

  private Vector3 GetGunPosition () {
    if (Sprite.IsPlaying("idle_gun"))
      return new Vector3(14, 10.5f, z) / 100f;
    else if (Sprite.IsPlaying("walk_gun"))
      return new Vector3(14, 10.5f, z) / 100f;
    else if (Sprite.IsPlaying("torpedo"))
      return new Vector3(14, 11.5f, z) / 100f;
    else if (Sprite.IsPlaying("in_ship"))
      return new Vector3(16, 4, z) / 100f;
    else if (Sprite.IsPlaying("wall_slide_shoot"))
      return new Vector3(16, 12.5f, z) / 100f;
    else
      return new Vector3(14, 11.5f, z) / 100f;
  }

  private void StopFlying() {
    Sprite.StopBlur();
    this.flying = false;
    Physics.vspeed = this.jumpSpeed;
    Game.Create("Ship", transform.position);
  }

  /***********************************
   * INPUT HANDLERS
   **********************************/

  protected override void UpHeld(float val) {
    if (Is("Climbing"))
      Physics.vspeed += Physics.Climb.acceleration;
    else if (Is("Flying"))
      Physics.vspeed += accelerationSpeed;
  }

  protected override void DownHeld(float val) {
    if (Is("Climbing"))
      Physics.vspeed -= Physics.Climb.acceleration;
    else if (Is("Flying"))
      Physics.vspeed -= accelerationSpeed;
  }

  protected override void LeftHeld (float val) {
    if (Is("Rolling") || Is("Torpedoing") || Is("Hurt"))
      return;

    if (Is("Climbing")) {
        Physics.hspeed -= Physics.Climb.acceleration;
        return;
    }

    Physics.hspeed -= this.accelerationSpeed;
  }

  protected override void RightHeld (float val) {
    if (Is("Rolling") || Is("Torpedoing") || Is("Hurt"))
      return;

    if (Is("Climbing")) {
        Physics.hspeed += Physics.Climb.acceleration;
        return;
    }

    Physics.hspeed += this.accelerationSpeed;
  }

  protected override void UnoPressed () {
    if (Is("Rolling") || Is("Torpedoing") || Is("Hurt"))
      return;

    if (Is("Flying")) {
      StopFlying();
      return;
    }

    if (Is("Swimming")) {
      Physics.Swim.Stroke();
      SolidPhysics.Collider.ClearFooting();
    } else if (Is("WallSliding")) {
      SolidPhysics.Walljump.ActuallyWalljump();
    } else if (HasFooting || Is("Climbing")) {
      Physics.vspeed = this.jumpSpeed;
      SolidPhysics.Collider.ClearFooting();
      Physics.Climb.Stop();
    } else if (canDoubleJump)
      State("DoubleJump");
  }

  protected override void UnoReleased () {
    if (Is("Flying") || Is("Swimming"))
      return;

    if (Physics.vspeed > 2)
      Physics.vspeed = 2;
  }

  protected override void DosPressed () {
    State("Shoot");
  }

  protected override void DosHeld () {
    if (!Is("Shooting"))
      State("Shoot");
    if (!Is("Shooting"))
      return;

    // Reset timer until it's released
    ShootTimer.Enabled = false;
    ShootTimer.Enabled = true;

    GrowChargeAura();
  }

  protected override void DosReleased () {
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

    chargeLight.GetComponent<Light>().intensity = Math.Max(chargeLight.GetComponent<Light>().intensity, 0.5f);
    GameObject laserLight = Instantiate(chargeLight) as GameObject;
    laserLight.transform.parent = laser.transform;
    laserLight.transform.localPosition = new Vector3(0, 0, 0);

    ResetChargeAura();
  }

  protected override void TresPressed () {
    State("Torpedo");
  }

  protected override void CuatroPressed () {
    // State("Shield");
  }

  protected override void LeftTriggerPressed() {
    State("Roll", true);
  }

  protected override void RightTriggerPressed() {
    State("Roll", false);
  }

  /***********************************
   * STATE CHANGE FUNCTIONS
   **********************************/

  public void StateRoll(bool rollLeft) {
    if (!HasFooting || RollTimer.Enabled || Is("Rolling"))
      return;

    facingLeftBeforeRoll = Sprite.FacingLeft;
    Sprite.FacingLeft = rollLeft;

    ShootTimer.Enabled = false;
    ResetChargeAura();

    Sprite.Play("Roll");
    Physics.hspeedMax = 5.5f;

    if (Sprite.FacingLeft)
      Physics.hspeed = -Physics.hspeedMax;
    else if (Sprite.FacingRight)
      Physics.hspeed = Physics.hspeedMax;
  }

  public void StateDoubleJump() {
    canDoubleJump = false;
    Physics.vspeed = this.doubleJumpSpeed;
    Game.CreateParticle("AirRipple", Mask.Center);
  }

  public void StateShoot() {
    if (Is("Rolling") || Is("Swimming") || Is("Torpedoing") || Is("Climbing") || Is("Hurt"))
      return;

    ShootTimer.Enabled = false;
    ShootTimer.Enabled = true;
    ResetChargeAura();
  }

  public void StateClimb(Ladder_Base other) {
    Sprite.Play("Climb");
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

      if (Physics.vspeed < 0) {
        Game.CreateParticle("SplashTop", new Vector3(Mask.Center.x, other.Mask.Top + 0.05f, z));
      } else if (Physics.vspeed > 0)
        Game.CreateParticle("SplashBottom", new Vector3(Mask.Center.x, other.Mask.Bottom + 0.05f, z));

      Physics.vspeed = Math.Max(Physics.vspeed, -1);
      Physics.hspeed = Math.Min(Physics.hspeed, 1);
      Physics.hspeed = Math.Max(Physics.hspeed, -1);
    }
  }

  public void StateTorpedo() {
    if (Is("Torpedoing") || Is("Rolling") || Is("Climbing") || Is("Hurt"))
      return;

    ShootTimer.Enabled = false;
    ResetChargeAura();

    if (!Is("Flying"))
      Sprite.Play("Torpedo");

    chargeAura.transform.localPosition = GetGunPosition();
    Torpedo_Base torpedo = Game.Create("Torpedo", chargeAura.transform.position) as Torpedo_Base;

    torpedo.Sprite.FacingLeft = Sprite.FacingLeft;
    torpedo.Sprite.SetAngle(Sprite.currentRotationAngle);

    float destX = Sprite.FacingRight ? 4f : -4f;
    float destY = (float)Math.Tan(Math.PI * Sprite.currentRotationAngle / 180f) * destX;
    torpedo.Physics.MoveTo(new Vector2(chargeAura.transform.position.x + destX, chargeAura.transform.position.y + destY), 1f);

    if (!Is("Flying")) {
      Physics.hspeed = -torpedo.Physics.hspeed * 2;
      if (!HasFooting)
        Physics.vspeed = -torpedo.Physics.vspeed * 2;
      TorpedoTimer.Enabled = true;
    }
  }

  public void StateFlying() {
    Sprite.Play("Flying");
    ShootTimer.Enabled = false;
    ResetChargeAura();
    canDoubleJump = true;
    Sprite.StopBlur();
    Sprite.StartBlur(0.001f, 0.2f, 0.02f);
    Physics.Swim.Stop();

    flying = true;
    Physics.hspeed = 0;
    Physics.vspeed = 0;
  }

  public void StateHurt(bool moveLeft, int damage) {
    if (Is("Hurt") || Is("Invincible"))
      return;

    Game.HUD.ReduceShield(damage);
    Sprite.FacingLeft = !moveLeft;

    ShootTimer.Enabled = false;
    ResetChargeAura();

    Sprite.Play("Hurt");
    Physics.vspeed = 0;

    if (Sprite.FacingLeft)
      Physics.hspeed = 2;
    else if (Sprite.FacingRight)
      Physics.hspeed = -2;

    HurtTimer.Enabled = true;
    InvincibleTimer.Enabled = true;

    Game.CreateParticle("Blood", Mask.Center);
  }

  /***********************************
   * STATE CHECKERS
   **********************************/

  public bool IsRolling() { return Sprite.IsPlaying("roll"); }
  public bool IsShooting() { return ShootTimer.Enabled; }
  public bool IsClimbing() { return Physics.Climbing; }
  public bool IsSwimming() { return Physics.Swimming; }
  public bool IsTorpedoing() { return TorpedoTimer.Enabled; }
  public bool IsFlying() { return flying; }
  public bool IsWallSliding() { return SolidPhysics.Walljump.Sliding; }
  public bool IsHurt() { return HurtTimer.Enabled; }
  public bool IsInvincible() { return InvincibleTimer.Enabled; }

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

  public Timer RollTimer { get { return Timer4; } }
  protected override void Timer4Elapsed(object source, ElapsedEventArgs e) {
    RollTimer.Enabled = false;
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
