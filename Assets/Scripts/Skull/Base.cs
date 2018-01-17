using UnityEngine;
using System;
using System.Collections;

namespace Skull {
  public class Base : EnemyObj {
    public float acceleration = 0.2f;
    private eDirection direction;
    private bool invincible = false;

    protected override void LoadReferences() {
      Sprite = new Sprite();
      Sprite.enabled = true;
      base.LoadReferences();
    }

    protected override void Init() {
      if (Physics.hspeed > 0)
        direction = eDirection.RIGHT;
      else if (Physics.hspeed < 0)
        direction = eDirection.LEFT;
      else if (Sprite.mirrorToStart)
        direction = eDirection.LEFT;
      else
        direction = eDirection.RIGHT;
    }

    protected override void Step() {
      if (!HasFooting) {
        Physics.SkipNextFrictionUpdate();

        if (direction == eDirection.LEFT) {
          Physics.hspeed -= acceleration;
        } else {
          Physics.hspeed += acceleration;
        }
      }
    }

    public void ChangeDirection() {
      if (direction == eDirection.LEFT)
        direction = eDirection.RIGHT;
      else
        direction = eDirection.LEFT;
    }

    public void CreateWalkPuffs(int count) {
      for (int i = 0; i < count; i++)
        Game.CreateParticle("Puff", Mask.Center);
    }

    public void Jump() {
      Physics.vspeed = 2;
    }

    /***********************************
     * STATE CHANGE FUNCTIONS
     **********************************/

    public void StateInvincible() {
      invincible = true;
      StartCoroutine("StopInvincible");
    }

    public void StateDie() {
      Sound.Play("Die");
      Game.CreateParticle("SkullDie", Mask.Center);
      DestroySelf();
    }

    /***********************************
     * STATE CHECKERS
     **********************************/

    public bool IsInvincible() { return invincible; }

    /***********************************
     * CO-ROUTINES
     **********************************/

    private IEnumerator StopInvincible() {
      yield return new WaitForSeconds(0.5f);
      invincible = false;
    }

  }
}
