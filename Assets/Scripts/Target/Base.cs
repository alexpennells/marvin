using UnityEngine;
using System;
using System.Collections;

namespace Target {
  public class Base : BaseObj {
    // Whether this is a basic target or will attack the player.
    public bool posessed = true;

    private Player.Base player;
    private ParticleSystem tornado;
    private Vector2 attackRange = new Vector2(1.7f, 0.6f);
    private bool appearDelay = false;
    private bool moveUp = false;

    public override void LoadReferences() {
      player = GameObject.Find("Player").GetComponent<Player.Base>();
      tornado = transform.Find("Tornado").GetComponent<ParticleSystem>();

      Sprite = new Sprite();
      Sound = new Sound();
      base.LoadReferences();
    }

    public override void Init() {
      Physics.vspeed = (float)Game.Random.Next(0, 101) / 400f - 0.125f;
      State("Idle");
      base.Init();
    }

    public override void Step() {
      if (moveUp)
        Physics.vspeed += 0.005f;
      else
        Physics.vspeed -= 0.005f;

      if (Physics.vspeed > 0.125f)
        moveUp = false;
      else if (Physics.vspeed < -0.125f)
        moveUp = true;

      if (Is("Posessed") && !Is("Broken")) {
        float xDist = (float)VectorLib.GetEdgeDistanceX(this, player);
        float yDist = (float)VectorLib.GetDistanceY(this, player);

        if (xDist < attackRange.x && yDist < attackRange.y && !player.Is("Dead")) {
          State("Stretch");
        } else {
          State("Shrink");
        }
      }

      base.Step();
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

      Sound.Play("Appear");
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

      Sound.StopLoop("Wind");
      Sprite.Play("Shrink");
    }

    public void StateInhale() {
      Sprite.Play("Inhale");
      Sound.StartLoop("Wind");
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
