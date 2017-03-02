using UnityEngine;
using System;
using System.Collections;

public class Player_Sprite : SpriteObj {

  public override void Step() {
    if (Base.Is("Climbing") && !AdditiveExists("tail"))
      CreateAdditive("tail", "climbing_tail", new Vector2(0, 0));
    else if (!Base.Is("Climbing") && AdditiveExists("tail"))
      DeleteAdditive("tail");

    if (Base.Is("Torpedoing"))
      return;

    if (Base.HasFooting)
      FaceFooting();
    else {
      float angle = Base.Physics.vspeed * 2;
      if (Base.Is("Flying"))
        angle *= 2;

      if (FacingLeft)
        angle *= -1;
      FaceAngle(angle);
    }

    if (Base.Is("Pouncing"))
      return;

    if (Game.LeftHeld)
      FacingLeft = true;
    else if (Game.RightHeld)
      FacingRight = true;

    if (Base.Is("Flying")) {
      Play("Flying");
      return;
    }

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
      Animate("idle_gun", 0.5f);
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
    string spriteName = Base.Is("Shooting") ? "jump_gun" : "jump";

    if (Base.Physics.vspeed > 3)
      Animate(spriteName, 0f, 0f);
    else if (Base.Physics.vspeed > 0f)
      Animate(spriteName, 0f, 0.4f);
    else
      Animate(spriteName, 0f, 0.8f);
  }

  public void PlayDuck() {
    Animate("duck", 0.5f);
  }

  public void PlayPounce() {
    Animate("pounce", 1f);
  }

  public void PlayClimb() {
    Animate("climb", ClimbSpeed());
  }

  public void PlaySwim() {
    Animate("swim", SwimSpeed());
  }

  public void PlayTorpedo() {
    Animate("torpedo", 0f);
  }

  public void PlayFlying() {
    Animate("in_ship", 1f);
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
