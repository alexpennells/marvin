using UnityEngine;
using System;

namespace Rat {
  [RequireComponent (typeof (Sprite))]
  public class Base : EnemyObj {
    public float acceleration = 0.2f;
    private eDirection direction;

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
      if (!Is("Dead")) {
        if (direction == eDirection.LEFT) {
          Physics.hspeed -= acceleration;
        } else {
          Physics.hspeed += acceleration;
        }
      }
    }

    protected override void OffScreenStep() {
      if (Is("Dead"))
        DestroySelf();
    }

    public void ChangeDirection() {
      if (direction == eDirection.LEFT)
        direction = eDirection.RIGHT;
      else
        direction = eDirection.LEFT;
    }

    /***********************************
     * STATE CHANGE FUNCTIONS
     **********************************/

    public void StateDie() {
      if (Is("Dead"))
        return;

      Sprite.Play("Die");
      Physics.hspeed = -Physics.hspeed / 4;
      Physics.vspeed = 2f;
      Physics.gravity = 0.1f;
      Destroy(SolidPhysics.Collider);
    }

    /***********************************
     * STATE CHECKERS
     **********************************/

    public bool IsDead() { return Sprite.IsPlaying("die"); }
  }
}
