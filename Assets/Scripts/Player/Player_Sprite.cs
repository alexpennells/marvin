using UnityEngine;
using System;
using System.Collections;

public class Player_Sprite : SpriteObj {

  private bool deathFallComplete = false;

  public override void Step() {
    base.Step();

    if (Base.Is("Torpedoing") || Base.Is("Hurt"))
      return;

    if (Base.Is("WallSliding")) {
      Play("WallSlide");
      FaceAngle(0);
      return;
    }

    if (Base.HasFooting)
      FaceFooting();
    else {
      float angle = Base.Physics.vspeed * 2;
      if (Base.Is("Dead"))
        angle *= 2;

      if ((!Base.Is("Dead") && FacingLeft) || (Base.Is("Dead") && FacingRight))
        angle *= -1;
      FaceAngle(angle);
    }

    if (Base.Is("Dead")) {
      if (Base.HasFooting && deathFallComplete)
        Play("DieLanding");
      return;
    }

    if (Game.LeftHeld && !Game.paused)
      FacingLeft = true;
    else if (Game.RightHeld && !Game.paused)
      FacingRight = true;

    if (Base.Is("Climbing")) {
      Play("Climb");
      return;
    }

    if (Base.Is("Slashing"))
      return;

    if (Base.HasFooting) {
      if (Game.paused) {
        // If the game is paused, just continue the current animation
        if (IsPlaying("walk", "walk_gun", "walk_reaper"))
          Play("Walk");
        else if (IsPlaying("idle", "idle_gun", "idle_reaper"))
          Play("Idle");
      } else {
        if (Game.LeftHeld || Game.RightHeld) {
          Play("Walk");
        } else {
          Play("Idle");
        }
      }
    } else
      Play("Jump");
  }

  public override void FireAnimationEndHandlers() {
    if (animationEndSideSlash)
      AnimationEndSideSlashHandler();

    if (animationEndDeathFall)
      AnimationEndDeathFallHandler();
  }

  public override void ToggleAdditives() {
    ToggleGunAdditives();

    if (Base.Is("Climbing") && !AdditiveExists("climb_tail"))
      CreateAdditive("climb_tail", "climbing_tail", new Vector2(0, 0), 1);
    else if (!Base.Is("Climbing") && AdditiveExists("climb_tail"))
      DeleteAdditive("climb_tail");
  }

  /***********************************
   * ANIMATION DEFINITIONS
   **********************************/

  public void PlayIdle() {
    if (Base.Is("Reaper"))
      Animate("idle_reaper", 0.5f);
    else if (Base.Is("Shooting"))
      Animate("idle_gun", 0.5f);
    else
      Animate("idle", 0.5f);
  }

  public void PlayWalk() {
    if (Base.Is("Reaper"))
      Animate("walk_reaper", WalkSpeed());
    else if (Base.Is("Shooting"))
      Animate("walk_gun", WalkSpeed());
    else
      Animate("walk", WalkSpeed());
  }

  public void PlayJump() {
    string spriteName = "jump";
    if (Base.Is("Reaper"))
      spriteName = "jump_reaper";
    else if (Base.Is("Shooting"))
      spriteName = "jump_gun";

    if (Base.Physics.vspeed > 3)
      Animate(spriteName, 0f, 0f);
    else if (Base.Physics.vspeed > 0f)
      Animate(spriteName, 0f, 0.4f);
    else
      Animate(spriteName, 0f, 0.8f);
  }

  public void PlayClimb() {
    Animate("climb", ClimbSpeed());
  }

  public void PlayTorpedo() {
    Animate("torpedo", 0f);
  }

  public void PlayWallSlide() {
    string spriteName = "wall_slide";
    if (Base.Is("Reaper"))
      spriteName = "wall_slide_reaper";
    else if (Base.Is("Shooting"))
      spriteName = "wall_slide_shoot";

    if (Base.Physics.vspeed > 3)
      Animate(spriteName, 0f, 0f);
    else if (Base.Physics.vspeed > 0f)
      Animate(spriteName, 0f, 0.4f);
    else
      Animate(spriteName, 0f, 0.8f);
  }

  public void PlayHurt() {
    Animate("hurt", 0f);
  }

  public void PlayDie() {
    Animate("die_fall", 1f);
  }

  public void PlayDieLanding() {
    Animate("die_land", 1f);
  }

  public void PlaySlash() {
    (Base as Player_Base).QueuedSlash = false;

    if (IsPlaying("slash_side_one"))
      Animate("slash_side_two", 1f);
    else
      Animate("slash_side_one", 1f);
  }

  /***********************************
   * ANIMATION END DEFINITIONS
   **********************************/

  private bool animationEndDeathFall = false;
  public bool AnimationEndDeathFall { set { animationEndDeathFall = true; } }
  private void AnimationEndDeathFallHandler() {
    animationEndDeathFall = false;
    deathFallComplete = true;
  }

  private bool animationEndSideSlash = false;
  public bool AnimationEndSideSlash { set { animationEndSideSlash = true; } }
  private void AnimationEndSideSlashHandler() {
    animationEndSideSlash = false;
    if ((Base as Player_Base).QueuedSlash)
      Play("Slash");
    else
      Play("Idle");
  }

  /***********************************
   * PRIVATE DEFINITIONS
   **********************************/

  private float WalkSpeed() {
    return 3f - Math.Abs(Base.Physics.hspeed) / Base.Physics.hspeedMax;
  }

  private float ClimbSpeed() {
    return Math.Max(Math.Abs(Base.Physics.vspeed), Math.Abs(Base.Physics.hspeed)) / 4;
  }

  private void ToggleGunAdditives() {
    string currentGun = (Base as Player_Base).CurrentGun;
    if (currentGun != "laser" && AdditiveExists("laser"))
      DeleteAdditive("laser");
    if (currentGun != "launcher" && AdditiveExists("launcher"))
      DeleteAdditive("launcher");
    if (currentGun != "pistol" && AdditiveExists("pistol"))
      DeleteAdditive("pistol");
    if (currentGun != "steamer" && AdditiveExists("steamer"))
      DeleteAdditive("steamer");

    if (!AdditiveExists(currentGun) && IsPlaying("idle_gun", "walk_gun", "jump_gun", "torpedo", "wall_slide_shoot"))
      CreateAdditive(currentGun, currentGun, new Vector2(0.07f, 0.083f), 1);

    if (AdditiveExists(currentGun)) {
      if (IsPlaying("idle_gun"))
        Additive(currentGun).Position(0.04f, 0.08f);
      else if (IsPlaying("torpedo"))
        Additive(currentGun).Position(0.02f, 0.09f);
      else if (IsPlaying("wall_slide_shoot"))
        Additive(currentGun).Position(0.05f, 0.1f);
      else if (IsPlaying("walk_gun")) {
        if (GetAnimationTime() < 0.25f || GetAnimationTime() > 0.75f)
          Additive(currentGun).Position(0.04f, 0.07f);
        else
          Additive(currentGun).Position(0.04f, 0.08f);
      }
      else if (IsPlaying("jump_gun")) {
        if (GetAnimationTime() < 0.33f)
          Additive(currentGun).Position(0.02f, 0.1f);
        else
          Additive(currentGun).Position(0.02f, 0.09f);
      }
      else
        DeleteAdditive(currentGun);
    }
  }
}
