using UnityEngine;
using System;
using System.Collections;

public class Player_Sprite : SpriteObj {

  public override void Step() {
    ToggleAdditives();

    if (Base.Is("Torpedoing"))
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
      if (Base.Is("Flying") || Base.Is("Swimming"))
        angle *= 2;

      if (FacingLeft)
        angle *= -1;
      FaceAngle(angle);
    }

    if (Base.Is("Rolling"))
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

    if (Base.Is("Swimming") && !Base.HasFooting) {
      Play("Swim");
      return;
    }

    if (Base.HasFooting) {
      StopBlur();

      if (Game.LeftHeld || Game.RightHeld) {
        Play("Walk");
      } else {
        Play("Idle");
      }
    } else
      Play("Jump");
  }

  public void RollEnd() {
    (Base as Player_Base).RollTimer.Enabled = true;

    Play("Idle");
    if (Base.HasFooting)
      Base.Physics.hspeed = 0;

    // Face the direction that the player was facing before the roll started.
    if ((Base as Player_Base).FacingLeftBeforeRoll)
      Base.Sprite.FacingLeft = true;
    else
      Base.Sprite.FacingRight = true;
  }

  /***********************************
   * ANIMATION DEFINITIONS
   **********************************/

  public void PlayIdle() {
    if (Base.Is("Shooting"))
      Animate("idle_gun", Base.Is("Swimming") ? 0.25f : 0.5f);
    else
      Animate("idle1", Base.Is("Swimming") ? 0.25f : 0.5f);
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

  public void PlayRoll() {
    Animate("roll", 1.25f);
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

  public void PlayWallSlide() {
    Animate(Base.Is("Shooting") ? "wall_slide_shoot" : "wall_slide", 1f);
  }

  /***********************************
   * PRIVATE DEFINITIONS
   **********************************/

  private float WalkSpeed() {
    float s = 3f - Math.Abs(Base.Physics.hspeed) / Base.Physics.hspeedMax;
    return s / (Base.Is("Swimming") ? 2f : 1f);
  }

  private float ClimbSpeed() {
    return Math.Max(Math.Abs(Base.Physics.vspeed), Math.Abs(Base.Physics.hspeed)) / 4;
  }

  private float SwimSpeed() {
    return Math.Min(Math.Max(Base.Physics.vspeed, 0.75f), 6f);
  }

  private void ToggleAdditives() {
    if (Base.Is("Climbing") && !AdditiveExists("climb_tail"))
      CreateAdditive("climb_tail", "climbing_tail", new Vector2(0, 0), 1);
    else if (!Base.Is("Climbing") && AdditiveExists("climb_tail"))
      DeleteAdditive("climb_tail");

    if (Base.Is("Swimming") && !Base.HasFooting && !AdditiveExists("swim_tail"))
      CreateAdditive("swim_tail", "swim_tail", new Vector2(0, 0), -1);
    else if ((!Base.Is("Swimming") || Base.HasFooting) && AdditiveExists("swim_tail"))
      DeleteAdditive("swim_tail");
  }
}
