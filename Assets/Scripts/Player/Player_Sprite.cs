using UnityEngine;
using System;
using System.Collections;

public class Player_Sprite : SpriteObj {

  public override void Step() {
    if (Base.HasFooting)
      FaceFooting();
    else
      FaceAngle(0);

    if (Base.Is("Sliding") && Math.Abs(Base.Physics.hspeed) < 2f) {
      Play("Duck");
      StopBlur();
    }

    if (Base.Is("Sliding")) {
      FaceFooting();
      return;
    }

    if (Game.LeftHeld)
      FacingLeft = true;
    else if (Game.RightHeld)
      FacingRight = true;

    if (Base.Is("Climbing")) {
      Play("Climb");
      return;
    }

    if (Base.Is("Swimming")) {
      Play("Swim");
      return;
    }

    if (Base.HasFooting) {
      StopBlur();

      if (Game.DownHeld) {
        Play("Duck");
      } else if (Game.LeftHeld || Game.RightHeld) {
        Play("Walk");
      } else {
        Play("Idle");
      }
    } else
      Play("Jump");
  }

  /***********************************
   * ANIMATION DEFINITIONS
   **********************************/

  public void PlayIdle() {
    if (Base.Is("Shooting"))
      Animate("idle_gun", 0f);
    else
      Animate("idle1", 0.5f);
  }

  public void PlayWalk() {
    if (Base.Is("Shooting"))
      Animate("walk_gun", WalkSpeed());
    else
      Animate("walk", WalkSpeed());
  }

  public void PlayJump() {
    if (Base.Is("Shooting"))
      Animate("jump_gun", 0f);
    else {
      if (Base.Physics.vspeed > 3)
        Animate("jump", 0f, 0f);
      else if (Base.Physics.vspeed > 0f)
        Animate("jump", 0f, 0.4f);
      else
        Animate("jump", 0f, 0.8f);
    }
  }

  public void PlayDuck() {
    Animate("duck", 0f);
  }

  public void PlaySlide() {
    Animate("slide", 0f);
  }

  public void PlayClimb() {
    Animate("climb", ClimbSpeed());
  }

  public void PlaySwim() {
    Animate("swim", SwimSpeed());
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

  private float SwimSpeed() {
    return Math.Min(Math.Max(Base.Physics.vspeed, 0.1f), 1f);
  }
}
