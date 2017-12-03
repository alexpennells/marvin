using UnityEngine;
using System;
using System.Collections;

namespace Player {
  public class Sprite : SpriteObj {

    private bool deathFallComplete = false;

    public override void Step() {
      base.Step();

      if (Base.Is("Hurt"))
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

      if (Base.HasFooting) {
        if (Game.paused) {
          // If the game is paused, just continue the current animation
          if (IsPlaying("walk"))
            Play("Walk");
          else if (IsPlaying("idle"))
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
      if (animationEndDeathFall)
        AnimationEndDeathFallHandler();
    }

    public override void ToggleAdditives() {
      if (Base.Is("Climbing") && !AdditiveExists("climbing_tail"))
        CreateAdditive("climbing_tail", "climbing_tail", new Vector2(0, 0), 1);
      else if (!Base.Is("Climbing") && AdditiveExists("climbing_tail"))
        DeleteAdditive("climbing_tail");
    }

    /***********************************
     * ANIMATION DEFINITIONS
     **********************************/

    public void PlayIdle() {
      Animate("idle", 0.5f);
    }

    public void PlayWalk() {
      if (Base.Is("Running"))
        Animate("run", RunSpeed());
      else
        Animate("walk", 0.25f);
    }

    public void PlayJump() {
      if (Base.Physics.vspeed > 3)
        Animate("jump", 0f, 0f);
      else if (Base.Physics.vspeed > 0f)
        Animate("jump", 0f, 0.4f);
      else
        Animate("jump", 0f, 0.8f);
    }

    public void PlayClimb() {
      Animate("climb", ClimbSpeed());
    }

    public void PlayWallSlide() {
      if (Base.Physics.vspeed > 3)
        Animate("slide", 0f, 0f);
      else if (Base.Physics.vspeed > 0f)
        Animate("slide", 0f, 0.4f);
      else
        Animate("slide", 0f, 0.8f);
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

    /***********************************
     * ANIMATION END DEFINITIONS
     **********************************/

    private bool animationEndDeathFall = false;
    public bool AnimationEndDeathFall { set { animationEndDeathFall = true; } }
    private void AnimationEndDeathFallHandler() {
      animationEndDeathFall = false;
      deathFallComplete = true;
    }

    /***********************************
     * PRIVATE DEFINITIONS
     **********************************/

    private float RunSpeed() {
      return 2.5f - Math.Abs(Base.Physics.hspeed) / Base.Physics.hspeedMax;
    }

    private float ClimbSpeed() {
      return Math.Max(Math.Abs(Base.Physics.vspeed), Math.Abs(Base.Physics.hspeed)) / 4;
    }
  }
}
