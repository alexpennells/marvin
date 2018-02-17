using UnityEngine;
using System;
using System.Collections;

namespace Player {
  public class Sprite : SpriteBlock {
    public Sprite() { enabled = true; }

    public override void Step() {
      base.Step();
      UpdateScale();

      if (Game.disableInput)
        return;

      if (Base.Is("Pounding")) {
        Play("Pound");
        return;
      }

      if (Base.Is("Ziplining")) {
        Play("Zipline");
        FacingRight = Base.Physics.hspeed >= 0;
        FaceAngle(20 * (Base.Physics.hspeed >= 0 ? 1 : -1));
        return;
      }

      if (Base.Is("WallSliding")) {
        Play("Slide");
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

      if (Base.Is("Dead") || Base.Is("Throwing"))
        return;

      if (Game.LeftHeld && !Game.paused)
        FacingLeft = true;
      else if (Game.RightHeld && !Game.paused)
        FacingRight = true;

      if (Base.HasFooting) {
        if (Game.paused) {
          // If the game is paused, just continue the current animation
          if (IsPlaying("walk"))
            Play("Walk");
          else if (IsPlaying("idle"))
            Play("Idle");
        } else {
          if (Base.Is("Ducking"))
            Play("Duck");
          else if (Game.LeftHeld || Game.RightHeld)
            Play("Walk");
          else
            Play("Idle");
        }
      } else if (!IsPlaying("spin", "spin_headless"))
        Play("Jump");
    }

    private void UpdateScale() {
      float desired = 1;
      if (Base.Is("Ducking"))
        desired = 0.8f;
      else if (Base.Physics.vspeed > 0)
        desired = 1 + Base.Physics.vspeed / 8;

      if (desired < scaleY)
        scaleY = Math.Max(desired, scaleY - 0.05f);
      else if (desired > scaleY)
        scaleY = Math.Min(desired, scaleY + 0.05f);
    }

    /***********************************
     * ANIMATION DEFINITIONS
     **********************************/

    public void PlayIdle() {
      Animate(Base.Is("Headless") ? "idle_headless" : "idle", 0.5f);
    }

    public void PlayDuck() {
      Animate(Base.Is("Headless") ? "duck_headless" : "duck", 0.5f);
    }

    public void PlayWalk() {
      if (Base.Is("Running"))
        Animate(Base.Is("Headless") ? "run_headless" : "run", RunSpeed());
      else
        Animate(Base.Is("Headless") ? "walk_headless" : "walk", 0.75f);
    }

    public void PlayJump() {
      string n = Base.Is("Headless") ? "jump_headless" : "jump";

      if (Base.Physics.vspeed > 3)
        Animate(n, 0f, 0f);
      else if (Base.Physics.vspeed > 0f)
        Animate(n, 0f, 0.4f);
      else
        Animate(n, 0f, 0.8f);
    }

    public void PlaySlide() {
      string n = Base.Is("Headless") ? "slide_headless" : "slide";

      if (Base.Physics.vspeed > 3)
        Animate(n, 0f, 0f);
      else if (Base.Physics.vspeed > 0f)
        Animate(n, 0f, 0.4f);
      else
        Animate(n, 0f, 0.8f);
    }

    public void PlayZipline() {
      Animate(Base.Is("Headless") ? "zipline_headless" : "zipline", 1f);
    }

    public void PlaySpin() {
      Animate(Base.Is("Headless") ? "spin_headless" : "spin", 1.5f);
    }

    public void PlayThrow() {
      Animate(Base.Is("Headless") ? "throw_headless" : "throw", 0f);
    }

    public void PlayFall() {
      Animate("fall_headless", 0.75f);
    }

    public void PlayRevive() {
      Animate("revive_headless", 0.25f);
    }

    public void PlayPound() {
      Animate(Base.Is("Headless") ? "pound_headless" : "pound", 0f);
    }

    /***********************************
     * PRIVATE DEFINITIONS
     **********************************/

    private float RunSpeed() {
      return 2.5f - Math.Abs(Base.Physics.hspeed) / Base.Physics.hspeedMax;
    }
  }
}
