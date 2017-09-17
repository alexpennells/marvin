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

  private string currentGun = "pistol";
  public string CurrentGun {
    get { return currentGun; }
    set {
      ShootTimer.Enabled = false;
      ShootTimer.Enabled = true;
      currentGun = value;
    }
  }

  private bool queuedSlash = false;
  public bool QueuedSlash { get { return queuedSlash; } set { queuedSlash = value; } }

  private SpriteObj chargeAura = null;
  private GameObject chargeLight = null;
  private bool deadTimerExpired = false;

  protected override void Init() {
    ShootTimer.Interval = 2000;
    TorpedoTimer.Interval = 200;
    HurtTimer.Interval = 250;
    InvincibleTimer.Interval = 800;
    DeadTimer.Interval = 1000;

    chargeAura = transform.Find("ChargeAura").gameObject.GetComponent<SpriteObj>();
    chargeLight = chargeAura.transform.Find("Light").gameObject;
    ResetChargeAura();
  }

  protected override void Step () {
    if (deadTimerExpired) {
      deadTimerExpired = false;
      Game.ChangeScene("MainHub", 0, "CatHead");
    }

    if (Is("Torpedoing")) {
      Physics.SkipNextGravityUpdate();
      Physics.SkipNextFrictionUpdate();
    }

    if (Is("Slashing") && HasFooting) {
      if (Sprite.FacingLeft) {
        Physics.hspeed = Math.Min(Physics.hspeed, -0.5f);
        Physics.hspeed = Math.Max(Physics.hspeed, -2);
      }
      else if (Sprite.FacingRight) {
        Physics.hspeed = Math.Max(Physics.hspeed, 0.5f);
        Physics.hspeed = Math.Min(Physics.hspeed, 2);
      }
    }

    base.Step();

    if (HasFooting)
      canDoubleJump = true;
  }

  public void StartDeadTimer() {
    DeadTimer.Enabled = true;
  }

  private void ResetChargeAura () {
    chargeAura.SetAlpha(0f);
    chargeAura.SetSpeed(1.5f);
    chargeAura.transform.localScale = Vector3.zero;

    chargeLight.SetActive(false);
  }

  /***********************************
   * PROJECTILE CREATION LOGIC
   **********************************/

  private Vector3 BulletGlobalPosition () {
    return chargeAura.transform.position;
  }

  private Vector3 BulletLocalPosition () {
    if (Sprite.IsPlaying("idle_gun"))
      return new Vector3(14, 10.5f, z) / 100f;
    else if (Sprite.IsPlaying("walk_gun"))
      return new Vector3(14, 10.5f, z) / 100f;
    else if (Sprite.IsPlaying("torpedo"))
      return new Vector3(14, 11.5f, z) / 100f;
    else if (Sprite.IsPlaying("wall_slide_shoot"))
      return new Vector3(16, 12.5f, z) / 100f;
    else
      return new Vector3(14, 10.5f, z) / 100f;
  }

  private void CreateBullet() {
    GameObject p = Game.CreateParticle("Gunshot", Vector3.zero);
    p.transform.parent = transform;
    p.transform.localPosition = BulletLocalPosition();

    Bullet_Base bullet = Game.Create("Bullet", BulletGlobalPosition()) as Bullet_Base;
    bullet.Sprite.SetAngle(Sprite.currentRotationAngle);

    float destX = Sprite.FacingRight ? 1f : -1f;
    float destY = (float)Math.Tan(Math.PI * Sprite.currentRotationAngle / 180f) * destX;
    bullet.Physics.MoveTo(new Vector2(chargeAura.transform.position.x + destX, chargeAura.transform.position.y + destY), 16f);
  }

  private void CreateTorpedo() {
    Torpedo_Base torpedo = Game.Create("Torpedo", BulletGlobalPosition()) as Torpedo_Base;
    torpedo.Sprite.FacingLeft = Sprite.FacingLeft;
    torpedo.Sprite.SetAngle(Sprite.currentRotationAngle);

    float destX = Sprite.FacingRight ? 4f : -4f;
    float destY = (float)Math.Tan(Math.PI * Sprite.currentRotationAngle / 180f) * destX;
    torpedo.Physics.MoveTo(new Vector2(chargeAura.transform.position.x + destX, chargeAura.transform.position.y + destY), 1f);

    Physics.hspeed = -torpedo.Physics.hspeed * 2;
    if (!HasFooting)
      Physics.vspeed = -torpedo.Physics.vspeed * 2;
  }

  private void CreateLaser() {
    Laser_Base laser = Game.Create("Laser", BulletGlobalPosition()) as Laser_Base;
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

  private void UpdateChargeAuraPosition () {
    chargeAura.transform.localPosition = BulletLocalPosition();
    chargeAura.FacingLeft = Sprite.FacingLeft;
  }

  private void GrowChargeAura () {
    float chargeSpeed = 50f;
    chargeAura.SetAlpha(Math.Min(1f, chargeAura.GetAlpha() + 1f / chargeSpeed));

    float scaleSize = chargeAura.transform.localScale.y;
    scaleSize = Math.Min(0.8f, scaleSize + 0.8f / chargeSpeed);
    chargeAura.transform.localScale = new Vector3(scaleSize, scaleSize, scaleSize);
    UpdateChargeAuraPosition();

    chargeLight.SetActive(true);
    chargeLight.GetComponent<Light>().intensity = scaleSize * 3;
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
    if (Is("Torpedoing") || Is("Hurt") || Is("Dead"))
      return;

    if (Is("Climbing")) {
      Physics.hspeed -= Physics.Climb.acceleration;
      return;
    }

    Physics.hspeed -= this.accelerationSpeed;
  }

  protected override void RightHeld (float val) {
    if (Is("Torpedoing") || Is("Hurt") || Is("Dead"))
      return;

    if (Is("Climbing")) {
      Physics.hspeed += Physics.Climb.acceleration;
      return;
    }

    Physics.hspeed += this.accelerationSpeed;
  }

  protected override void UnoPressed () {
    if (Is("Torpedoing") || Is("Hurt") || Is("Dead"))
      return;

    if (Is("WallSliding")) {
      SolidPhysics.Walljump.ActuallyWalljump();
    } else if (HasFooting || Is("Climbing")) {
      Physics.vspeed = this.jumpSpeed;
      SolidPhysics.Collider.ClearFooting();
      Physics.Climb.Stop();
    } else if (canDoubleJump)
      State("DoubleJump");
  }

  protected override void UnoReleased () {
    if (Is("Dead"))
      return;

    if (Physics.vspeed > 2)
      Physics.vspeed = 2;
  }

  protected override void DosPressed () {
    if (Is("Reaper"))
      State("Slash");
    else
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

    if (currentGun == "laser")
      GrowChargeAura();
  }

  protected override void DosReleased () {
    if (Is("Shooting") && currentGun == "laser")
      CreateLaser();
  }

  protected override void TresPressed () {
  }

  protected override void CuatroPressed () {
  }

  protected override void LeftTriggerPressed() {
  }

  protected override void RightTriggerPressed() {
  }

  /***********************************
   * STATE CHANGE FUNCTIONS
   **********************************/

  public void StateDoubleJump() {
    canDoubleJump = false;
    Physics.vspeed = this.doubleJumpSpeed;
    Game.CreateParticle("AirRipple", Mask.Center);
  }

  public void StateShoot() {
    if (Is("Torpedoing") || Is("Climbing") || Is("Hurt") || Is("Dead"))
      return;

    ShootTimer.Enabled = false;
    ShootTimer.Enabled = true;
    UpdateChargeAuraPosition();

    switch (currentGun) {
      case "pistol":
        CreateBullet();
        break;
      case "launcher":
        Sprite.Play("Torpedo");
        CreateTorpedo();
        TorpedoTimer.Enabled = true;
        break;
      case "laser":
        ResetChargeAura();
        break;
    }
  }

  public void StateClimb(Ladder_Base other) {
    Sprite.Play("Climb");
    ShootTimer.Enabled = false;
    ResetChargeAura();
    canDoubleJump = true;
    Sprite.StopBlur();
    Physics.Climb.Begin(other);
  }

  public void StateHurt(bool moveLeft, int damage) {
    if (Is("Hurt") || Is("Invincible") || Is("Dead"))
      return;

    Game.HUD.ReduceShield(damage);
    Sprite.FacingLeft = !moveLeft;

    ShootTimer.Enabled = false;
    ResetChargeAura();

    Physics.vspeed = 0;

    if (Sprite.FacingLeft)
      Physics.hspeed = 2;
    else if (Sprite.FacingRight)
      Physics.hspeed = -2;

    Game.CreateParticle("Blood", Mask.Center);

    if (Game.HUD.IsDead()) {
      State("Die");
    } else {
      Sprite.Play("Hurt");
      HurtTimer.Enabled = true;
      InvincibleTimer.Enabled = true;
    }
  }

  public void StateDie() {
    Sprite.Play("Die");
    Physics.vspeed = 2;
    Stitch.CurShield = 1;
  }

  public void StateSlash() {
    if (Is("Slashing"))
      QueuedSlash = true;
    else
      Sprite.Play("Slash");
  }

  /***********************************
   * STATE CHECKERS
   **********************************/

  public bool IsReaper() { return currentGun == "scythe"; }
  public bool IsSlashing() { return Sprite.IsPlaying("slash_side_one", "slash_side_two"); }
  public bool IsShooting() { return ShootTimer.Enabled; }
  public bool IsClimbing() { return Physics.Climbing; }
  public bool IsTorpedoing() { return TorpedoTimer.Enabled; }
  public bool IsWallSliding() { return SolidPhysics.Walljump.Sliding; }
  public bool IsHurt() { return HurtTimer.Enabled; }
  public bool IsInvincible() { return InvincibleTimer.Enabled; }
  public bool IsDead() { return Sprite.IsPlaying("die_fall", "die_land"); }

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
