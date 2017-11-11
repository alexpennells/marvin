using UnityEngine;
using System;
using System.Timers;

namespace Target {
  [RequireComponent (typeof (Sprite))]
  [RequireComponent (typeof (Sound))]
  public class Base : BaseObj {
    // Whether this is a basic target or will attack the player.
    public bool posessed = true;

    private Player.Base player;
    private Vector2 attackRange = new Vector2(1.5f, 0.8f);

    protected override void LoadReferences() {
      player = GameObject.Find("Player").GetComponent<Player.Base>();
    }

    protected override void Init() {
      State("Idle");
      AppearTimer.Interval = 2000;
      DisappearTimer.Interval = 1000;
    }

    protected override void Step() {
      if (Is("Posessed") && !Is("Broken")) {
        float xDist = (float)VectorLib.GetEdgeDistanceX(this, player);
        float yDist = (float)VectorLib.GetDistanceY(this, player);

        if (xDist < attackRange.x && yDist < attackRange.y) {
          State("Stretch");
        } else {
          State("Shrink");
        }
      }
    }

    /***********************************
     * STATE CHANGE FUNCTIONS
     **********************************/

    public void StateIdle() {
      Sprite.Play("Idle");
    }

    public void StateStretch() {
      if (Is("Inhaling") || Is("Shrinking"))
        return;

      // Can't stretch out if recently damaged.
      if (AppearTimer.Enabled)
        return;

      DisappearTimer.Enabled = false;
      DisappearTimer.Enabled = true;

      Sprite.Play("Stretch");
    }

    public void StateHurt() {
      DisappearTimer.Enabled = false;
      AppearTimer.Enabled = false;
      AppearTimer.Enabled = true;
      State("Shrink");
    }

    public void StateShrink() {
      if (Is("Idle"))
        return;

      // Don't shrink if just done stretching.
      if (DisappearTimer.Enabled)
        return;

      Sprite.Play("Shrink");
    }

    public void StateInhaling() {
      Sprite.Play("Inhale");
    }

    public void StateBreak() {
      if (Is("Broken"))
        return;

      Sprite.Play("Break");
      Sound.Play("Break");
      Game.QueueFreezeFrame();
    }

    /***********************************
     * STATE CHECKERS
     **********************************/

    public bool IsPosessed() { return posessed; }
    public bool IsIdle() { return Sprite.IsPlaying("idle"); }
    public bool IsStretching() { return Sprite.IsPlaying("stretch"); }
    public bool IsInhaling() { return Sprite.IsPlaying("inhale"); }
    public bool IsShrinking() { return Sprite.IsPlaying("shrink"); }
    public bool IsBroken() { return Sprite.IsPlaying("break"); }

    /***********************************
     * TIMER HANDLERS
     **********************************/

    public Timer AppearTimer { get { return Timer1; } }
    protected override void Timer1Elapsed(object source, ElapsedEventArgs e) {
      AppearTimer.Enabled = false;
    }

    public Timer DisappearTimer { get { return Timer2; } }
    protected override void Timer2Elapsed(object source, ElapsedEventArgs e) {
      DisappearTimer.Enabled = false;
    }
  }
}
