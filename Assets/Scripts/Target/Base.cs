using UnityEngine;
using System;
using System.Collections;

namespace Target {
  [RequireComponent (typeof (Sprite))]
  [RequireComponent (typeof (Sound))]
  public class Base : EnemyObj {
    // Whether this is a basic target or will attack the player.
    public bool posessed = true;

    private Player.Base player;
    private ParticleSystem tornado;
    private Vector2 attackRange = new Vector2(1.8f, 0.8f);
    private bool appearDelay = false;

    protected override void LoadReferences() {
      player = GameObject.Find("Player").GetComponent<Player.Base>();
      tornado = transform.Find("Tornado").GetComponent<ParticleSystem>();
    }

    protected override void Init() {
      State("Idle");
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
      if (Is("Inhaling") && tornado.isStopped)
        tornado.Play();

      if (Is("Inhaling") || Is("Shrinking"))
        return;

      // Can't stretch out if recently damaged.
      if (appearDelay)
        return;

      Sprite.Play("Stretch");
    }

    public void StateHurt() {
      tornado.Stop();
      tornado.Clear();
      RestartCoroutine("AppearDelay");
      State("Shrink");
    }

    public void StateShrink() {
      if (Is("Idle"))
        return;

      // Don't shrink until the tornado is gone.
      tornado.Stop();
      if (tornado.IsAlive())
        return;

      Sprite.Play("Shrink");
    }

    public void StateInhale() {
      Sprite.Play("Inhale");
      tornado.Play();
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
     * CO-ROUTINES
     **********************************/

    private IEnumerator AppearDelay() {
      appearDelay = true;
      yield return new WaitForSeconds(2);
      appearDelay = false;
    }

  }
}
