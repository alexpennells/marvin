using UnityEngine;
using System;
using System.Collections;

namespace Rat {
  public class Base : BaseObj {
    public float acceleration = 0.2f;
    private eDirection direction;

    public override void LoadReferences() {
      Sprite = new Sprite();
      Sound = new Sound();
      Physics = new Physics(Physics);
      base.LoadReferences();
    }

    public override void Init() {
      StartCoroutine("Squeak");

      if (Physics.hspeed > 0)
        direction = eDirection.RIGHT;
      else if (Physics.hspeed < 0)
        direction = eDirection.LEFT;
      else if (Sprite.mirrorToStart)
        direction = eDirection.LEFT;
      else
        direction = eDirection.RIGHT;

      base.Init();
    }

    public override void Step() {
      if (!Is("Dead")) {
        if (direction == eDirection.LEFT) {
          Physics.hspeed -= acceleration;
        } else {
          Physics.hspeed += acceleration;
        }
      }

      base.Step();
    }

    public override void OffScreenStep() {
      if (Is("Dead"))
        DestroySelf();

      base.OffScreenStep();
    }

    public void ChangeDirection() {
      if (direction == eDirection.LEFT)
        direction = eDirection.RIGHT;
      else
        direction = eDirection.LEFT;
    }

    public void CreateWalkPuffs(int count) {
      if (Game.Camera.InView(this)) {
        Sound.Play("Footstep");
        for (int i = 0; i < count; i++)
          Game.CreateParticle("Puff", Position);
      }
    }

    /***********************************
     * STATE CHANGE FUNCTIONS
     **********************************/

    public void StateDie() {
      if (Is("Dead"))
        return;

      Sound.Play("Die");
      Sprite.Play("Die");
      Physics.hspeed = -Physics.hspeed / 4;
      Physics.vspeed = 2f;
      Physics.gravity = 0.1f;
      Physics.Ground.Collider.enabled = false;
    }

    /***********************************
     * STATE CHECKERS
     **********************************/

    public bool IsDead() { return Sprite.IsPlaying("die"); }

    /***********************************
     * CO-ROUTINES
     **********************************/

    private IEnumerator Squeak() {
      while (true) {
        Sound.Play("Squeak");
        yield return new WaitForSeconds(Game.Random.Next(1, 3));
      }
    }
  }
}
